$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$project = Join-Path $repoRoot 'src/ReachCommunityClient.App/ReachCommunityClient.App.csproj'
$output = Join-Path $repoRoot 'artifacts/win-x64'
$version = (Get-Content (Join-Path $repoRoot 'VERSION') -Raw).Trim()

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw '.NET 10 SDK is required. Install it from https://dotnet.microsoft.com/download/dotnet/10.0'
}

if (Test-Path $output) {
    Remove-Item $output -Recurse -Force
}
New-Item -ItemType Directory -Path $output -Force | Out-Null

dotnet publish $project `
    --configuration Release `
    --runtime win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:DebugType=embedded `
    "-p:Version=$version" `
    --output $output

if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed with exit code $LASTEXITCODE"
}

$exe = Join-Path $output 'ReachCommunityClient.exe'
if (-not (Test-Path $exe)) {
    throw "Publish completed but the expected executable was not found: $exe"
}

Write-Host "Windows x64 executable created: $exe" -ForegroundColor Green
