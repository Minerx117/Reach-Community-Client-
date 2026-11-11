$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
Set-Location $repoRoot

$version = (Get-Content (Join-Path $repoRoot 'VERSION') -Raw).Trim()
if ([string]::IsNullOrWhiteSpace($version)) {
    throw 'VERSION file is empty.'
}

function Find-DotNet {
    $command = Get-Command dotnet -ErrorAction SilentlyContinue
    if ($command) { return $command.Source }

    $defaultPath = Join-Path $env:ProgramFiles 'dotnet\dotnet.exe'
    if (Test-Path $defaultPath) { return $defaultPath }

    return $null
}

$dotnet = Find-DotNet
if (-not $dotnet) {
    $winget = Get-Command winget -ErrorAction SilentlyContinue
    if (-not $winget) {
        throw 'The .NET 10 SDK is not installed and WinGet is unavailable. Install the SDK from https://dotnet.microsoft.com/download/dotnet/10.0, then run BUILD-WINDOWS.cmd again.'
    }

    $answer = Read-Host 'The .NET 10 SDK is required. Install it now using Microsoft WinGet? (Y/N)'
    if ($answer -notmatch '^(Y|YES)$') {
        throw 'Build cancelled. Install the .NET 10 SDK and run BUILD-WINDOWS.cmd again.'
    }

    & $winget.Source install --id Microsoft.DotNet.SDK.10 --exact --accept-source-agreements --accept-package-agreements
    if ($LASTEXITCODE -ne 0) {
        throw "WinGet could not install the .NET 10 SDK (exit code $LASTEXITCODE). Install it manually from https://dotnet.microsoft.com/download/dotnet/10.0 and rerun this script."
    }

    $dotnetFolder = Join-Path $env:ProgramFiles 'dotnet'
    if (Test-Path $dotnetFolder) {
        $env:Path = "$dotnetFolder;$env:Path"
    }

    $dotnet = Find-DotNet
    if (-not $dotnet) {
        throw 'The .NET SDK installer completed, but dotnet.exe was not found. Close this window, open a new one, and run BUILD-WINDOWS.cmd again.'
    }
}

$sdkList = & $dotnet --list-sdks
if ($LASTEXITCODE -ne 0 -or -not ($sdkList | Where-Object { $_ -match '^10\.' })) {
    throw 'The .NET 10 SDK is required. Install it from https://dotnet.microsoft.com/download/dotnet/10.0 and run BUILD-WINDOWS.cmd again.'
}

$solution = Join-Path $repoRoot 'ReachCommunityClient.sln'
$project = Join-Path $repoRoot 'src/ReachCommunityClient.App/ReachCommunityClient.App.csproj'
$publishDir = Join-Path $repoRoot 'artifacts/publish/win-x64'
$stageDir = Join-Path $repoRoot 'artifacts/package/Reach-Community-Client'
$zipPath = Join-Path $repoRoot "artifacts/Reach-Community-Client-$version-win-x64.zip"

Write-Host "`n== Reach Community Client ${version}: Windows x64 build ==" -ForegroundColor Cyan
Write-Host 'Restoring NuGet packages...'
& $dotnet restore $solution
if ($LASTEXITCODE -ne 0) { throw "dotnet restore failed with exit code $LASTEXITCODE" }

Write-Host 'Running automated tests...'
& $dotnet test --solution $solution --configuration Release
if ($LASTEXITCODE -ne 0) { throw "dotnet test failed with exit code $LASTEXITCODE" }

if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }
New-Item -ItemType Directory -Force -Path $publishDir | Out-Null

Write-Host 'Publishing a self-contained, single-file Windows executable...'
& $dotnet publish $project `
    --configuration Release `
    --runtime win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:DebugType=embedded `
    "-p:Version=$version" `
    --output $publishDir
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE" }

$exePath = Join-Path $publishDir 'ReachCommunityClient.exe'
if (-not (Test-Path $exePath)) { throw 'Publish finished without producing ReachCommunityClient.exe.' }

if (Test-Path $stageDir) { Remove-Item $stageDir -Recurse -Force }
New-Item -ItemType Directory -Force -Path $stageDir | Out-Null
Copy-Item $exePath $stageDir
Copy-Item (Join-Path $repoRoot 'LICENSE') $stageDir
Copy-Item (Join-Path $repoRoot 'THIRD-PARTY-NOTICES.md') $stageDir

$readme = @"
Reach Community Client $version - Windows x64

1. Extract this ZIP to a folder.
2. Run ReachCommunityClient.exe.
3. The app checks your Steam MCC and Halo: Reach files without modifying them.
4. Use Launch official MCC to hand off to the official Steam client.

This release does not yet load the original Reach main menu or provide a live server browser, multiplayer sessions, or dedicated server. Those are planned as RCC-owned implementations. No external Halo client or backend is downloaded, built, bundled, or launched.

This self-contained app does not require a separate .NET runtime. Steam MCC and the user's own Halo: Reach content are required. No Microsoft game files or maps are included.

Reach Community Client source is GPL-3.0-or-later. See LICENSE and THIRD-PARTY-NOTICES.md.
"@

Set-Content -Path (Join-Path $stageDir 'READ-ME.txt') -Value $readme -Encoding utf8

$artifactsDir = Split-Path -Parent $zipPath
New-Item -ItemType Directory -Force -Path $artifactsDir | Out-Null
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path (Join-Path $stageDir '*') -DestinationPath $zipPath -CompressionLevel Optimal

if (-not (Test-Path $zipPath)) { throw 'The release ZIP was not created.' }

Write-Host "`nBUILD SUCCEEDED" -ForegroundColor Green
Write-Host "Windows executable: $exePath"
Write-Host "ZIP package:        $zipPath" -ForegroundColor Green
Write-Host 'The ZIP is self-contained and includes the executable, licence, and read-me.'
