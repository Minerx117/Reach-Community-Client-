# Reach Community Client

**An independent, community-built launcher foundation for Halo: The Master Chief Collection (MCC), with a focus on Halo: Reach.**

> Early development build. The current app detects a Steam MCC installation and asks Steam to launch it (which opens MCC, not directly into Reach). Mod management, map management, community downloads, and standalone multiplayer are not implemented yet.

## What is included in this starter

- Cross-platform Avalonia desktop shell (Windows, macOS, Linux).
- Steam library discovery, including additional Steam libraries listed in `libraryfolders.vdf`.
- MCC installation detection using Steam App ID `976730` and its local app manifest.
- Steam launch hand-off using `steam://rungameid/976730` (Linux also has a Steam CLI fallback).
- Unit tests for Steam library parsing and MCC detection.
- GitHub Actions build-and-test workflow on Windows, macOS, and Linux.
- Contributor, security, and community guidelines.

## Requirements

- .NET 10 SDK.
- Steam installed on the computer.
- A user-installed copy of Halo: The Master Chief Collection.

Avalonia supports desktop development across Windows, macOS, and Linux. See the [Avalonia getting-started guide](https://docs.avaloniaui.net/docs/get-started/) and the [.NET download page](https://dotnet.microsoft.com/download/dotnet/10.0). The launcher UI is cross-platform; that does not mean MCC itself is officially supported or playable on every operating system. Game availability and compatibility remain controlled by Steam, Microsoft, and the user’s platform.

## Run locally

```bash
cd reach-community-client
dotnet restore ReachCommunityClient.sln
dotnet run --project src/ReachCommunityClient.App
```

After the project has been published to GitHub, contributors can clone the public repository URL shown on its GitHub page.

## Build and test

```bash
dotnet build ReachCommunityClient.sln --configuration Release
dotnet test ReachCommunityClient.sln --configuration Release
```

## Publish a self-contained build

Example for Windows x64:

```bash
dotnet publish src/ReachCommunityClient.App/ReachCommunityClient.App.csproj \
  --configuration Release \
  --runtime win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -o artifacts/win-x64
```

Other runtime identifiers include `osx-arm64`, `osx-x64`, `linux-x64`, and `linux-arm64`. Native packaging and platform-specific testing are still needed before calling these production-ready installers.

## Project structure

```text
.
├── .github/
│   ├── ISSUE_TEMPLATE/
│   ├── workflows/ci.yml
│   └── pull_request_template.md
├── docs/
│   ├── ARCHITECTURE.md
│   └── PUBLISHING.md
├── src/
│   ├── ReachCommunityClient.App/       # Avalonia desktop UI
│   └── ReachCommunityClient.Core/      # Steam/MCC logic
├── tests/
│   └── ReachCommunityClient.Core.Tests/
├── CONTRIBUTING.md
├── CODE_OF_CONDUCT.md
├── LICENSE
├── Directory.Build.props
└── Directory.Packages.props
```

## Contributing

Issues, documentation improvements, tests, design feedback, and code contributions are welcome. Read [CONTRIBUTING.md](CONTRIBUTING.md) before opening a pull request. All changes are reviewed in public pull requests. AI-assisted contributions are welcome when the contributor understands, verifies, and takes responsibility for the submitted work.

## Open-source licence

This project is licensed under **GNU General Public License v3.0 or later**. See [LICENSE](LICENSE).

## Legal notice

Reach Community Client is an unofficial community project and is not affiliated with, endorsed by, or sponsored by Microsoft, Xbox, 343 Industries, Halo Studios, or Valve. Halo, Halo: Reach, and MCC are trademarks and/or copyrighted works of their respective owners. This repository does not contain MCC game files or Microsoft game assets. Users must obtain and install the game independently. The project does not bypass DRM, anti-cheat, account restrictions, or platform protections.
