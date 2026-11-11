# Reach Community Client

**An open-source cross-platform launcher shell and community integration project for Halo: Reach.**

> Version 0.01.03 adds local Reach-content compatibility diagnostics and an optional launcher integration for the separately licensed Project Noble standalone backend. The Avalonia shell is cross-platform. The Project Noble native game backend currently targets Windows 10/11.

## What is included

- Avalonia desktop shell for Windows, macOS, and Linux.
- Steam library discovery and MCC app ID `976730` detection.
- Reach content diagnostics: `haloreach.dll`, `haloreach/maps/*.map`, `mainmenu.map`, MCC launch executables, Steam app build ID, and Reach DLL file version.
- Official Steam launch hand-off for MCC.
- Optional Project Noble backend detection and launch. When separately built, Project Noble provides the native Reach engine client, main menu, server browser, multiplayer/lobbies, and dedicated server described by its public README.
- A Windows helper script to obtain and build the optional backend from its public MIT-licensed source.
- GPL-3.0-or-later source licence, contribution guidelines, tests, and GitHub CI.

## Version 0.01.03 capability status

| Feature | Status |
|---|---|
| Detect Steam MCC installation | Implemented |
| Detect Reach DLL and map files | Implemented; needs testing against each user install |
| Report MCC build ID and Reach DLL file version | Implemented where file metadata is available |
| Launch official MCC via Steam | Implemented |
| Launch original Reach main-menu map | Delegated to optional Project Noble backend; requires backend build and compatible installed game files |
| Live server browser and multiplayer sessions | Provided by the optional Project Noble client, not implemented natively in RCC |
| Dedicated server | Optional Project Noble build target; not wrapped/managed by RCC yet |
| Embed the Reach renderer inside RCC | Not implemented |
| Native Reach engine on Linux/macOS | Not implemented; backend currently Windows-focused |

Do not interpret file-presence diagnostics as proof that a particular engine build is compatible. The native backend must be tested against the installed MCC/Reach build.

## Project Noble backend

Project Noble is used as an optional external backend, not copied into this repository. Its public repository describes a C++20 standalone client which loads `haloreach.dll` and maps from the user's own MCC installation and includes the main-menu map, Armory, server browser, lobbies, multiplayer modes and dedicated-server target. The public repository states its source is MIT-licensed. See [Project Noble backend instructions](docs/PROJECT-NOBLE-BACKEND.md) and [third-party notices](THIRD-PARTY-NOTICES.md).

On Windows 10/11, install Git, CMake and Visual Studio with the C++ desktop workload, then run `BUILD-PROJECT-NOBLE-BACKEND.cmd`. The optional binaries are copied to `%LOCALAPPDATA%\ReachCommunityClient\backends\ProjectNoble\`. The RCC launcher starts `noble-client.exe` with the documented `-map mainmenu -mode menu` arguments. The server browser and multiplayer UI open in the separate Project Noble window.

Project Noble's public README currently specifies Windows 10/11, Visual Studio 2026 with the C++ workload, CMake and a local Steam MCC install with Reach. Linux/macOS support in the RCC shell does not imply that the Reach engine backend works on those platforms.

## Steam installation requirements

MCC Steam App ID is `976730`; Halo: Reach is a separate DLC app (`1064220`) and Reach multiplayer has a separate content pack (`1097224`). The checker uses actual files in the local install as its primary compatibility signal: `haloreach/haloreach.dll` and `haloreach/maps/mainmenu.map`. If these files are missing, use Steam → MCC → Properties → DLC to install Halo: Reach, then verify the game files. See [SteamDB MCC DLC list](https://steamdb.info/app/976730/dlc/) and [Reach multiplayer depot manifest](https://steamdb.info/depot/1064226/manifests/).

## Build and test RCC

Requires .NET 10 SDK.

```bash
dotnet restore ReachCommunityClient.sln
dotnet build ReachCommunityClient.sln --configuration Release
dotnet test --solution ReachCommunityClient.sln --configuration Release
```

## Build the diagnostic C++ host

```bash
cmake -S engine-host -B engine-host/build -DCMAKE_BUILD_TYPE=Release
cmake --build engine-host/build --config Release
ctest --test-dir engine-host/build --output-on-failure
```

This small target is still a diagnostic scaffold. The actual game runtime is provided by the optional Project Noble backend.

## Project layout

```text
.github/                         CI, release workflow, issue templates
docs/                            architecture and backend integration notes
engine-host/                     C++20 diagnostic host scaffold
scripts/                         optional backend build script
src/ReachCommunityClient.App/    Avalonia desktop shell
src/ReachCommunityClient.Core/   Steam diagnostics and backend integration
tests/                            Core unit tests
backends/ProjectNoble/            local build output (git-ignored)
```

## Open-source contribution

Reach Community Client code is licensed under GNU GPL-3.0-or-later. Contributions are reviewed through public pull requests. AI-assisted contributions are welcome when the author understands, verifies, tests, and takes responsibility for the submitted code. The optional Project Noble dependency retains its separate MIT licence and notices.

## Legal notice

Reach Community Client is an unofficial fan project and is not affiliated with Microsoft, Xbox, Bungie, 343 Industries, Halo Studios, or Valve. Halo, Halo: Reach and MCC are trademarks and/or copyrighted works of their respective owners. No game files, maps or proprietary assets are included. Users must own and install MCC and Reach independently. This project does not bypass DRM, account restrictions, or anti-cheat protections.
