# Reach Community Client

**An independent, open-source cross-platform Halo: Reach community client project.**

Version **0.01.03** focuses on safe Steam/MCC installation discovery and the first original RCC runtime and networking architecture contracts. All application, runtime, and multiplayer components are intended to be developed and maintained within this repository.

## Project direction

Reach Community Client is intended to become its own client: an original RCC application, native runtime integration, original Reach main-menu experience, community server browser, multiplayer sessions, and (eventually) dedicated-server tooling. These components will be developed in this repository under its own contribution, testing, and review process.

## What this release actually does

- Cross-platform Avalonia desktop shell (Windows, macOS, Linux).
- Steam library discovery, including secondary Steam libraries.
- MCC Steam App ID `976730` detection.
- Read-only Reach content diagnostics: `haloreach.dll`, map files, `mainmenu.map`, MCC executables, Steam build ID, and Reach DLL file version where available.
- User-initiated hand-off to the official MCC Steam client.
- Initial RCC-owned C++20 engine-host diagnostic scaffold.
- Engine and multiplayer interfaces describing future RCC-owned implementations.
- GPL-3.0-or-later licensing, contribution guidelines, tests, and GitHub CI.

## Capability status (honest release status)

| Feature | Status |
|---|---|
| Detect Steam MCC | Implemented; needs testing on user systems |
| Detect Reach files and report build metadata | Implemented as read-only diagnostics; not a complete runtime compatibility test |
| Launch official MCC through Steam | Implemented |
| RCC-owned engine host process | Diagnostic scaffold only; does not load the game engine |
| Original Reach main menu in RCC | Not implemented |
| Live RCC community server directory | Not implemented |
| Host/join multiplayer sessions | Not implemented |
| Actual in-game networking and synchronisation | Not implemented |
| RCC dedicated server | Not implemented |

No screen, server list, or multiplayer status is simulated or presented as functional.

## Architecture

- `ReachCommunityClient.App`: Avalonia UI, installation diagnostics, and explicit user-initiated Steam launch.
- `ReachCommunityClient.Core`: Steam/MCC discovery, compatibility reports, runtime capability model, and RCC-owned multiplayer contracts.
- `ReachCommunityClient.Core.Tests`: unit tests for core behaviour and capability status.
- `engine-host`: RCC-owned C++20 diagnostic process; not a game engine yet.
- `docs`: architecture, IPC draft, roadmap, and product-scope notes.

The intended long-term split is a cross-platform UI and service layer communicating with RCC's own native runtime over a versioned, local IPC protocol. The runtime must validate the user's installed game build before loading or interacting with any game components.

## Steam installation requirements

MCC Steam App ID is `976730`. Halo: Reach content must also be installed through MCC's Steam DLC settings. RCC checks local file evidence such as `haloreach/haloreach.dll` and `haloreach/maps/mainmenu.map`; presence of these files is not proof that the engine build is supported.

The installation scan is read-only. It does not alter, patch, download, or redistribute game files. No Microsoft game files, maps, textures, audio, or other proprietary assets are included.

## Build and test RCC

Requires the .NET 10 SDK.

```bash
dotnet restore ReachCommunityClient.sln
dotnet build ReachCommunityClient.sln --configuration Release
dotnet test --solution ReachCommunityClient.sln --configuration Release
```

## Build the RCC native diagnostic host

Requires CMake 3.20+ and a C++20 compiler.

```bash
cmake -S engine-host -B engine-host/build -DCMAKE_BUILD_TYPE=Release
cmake --build engine-host/build --config Release
ctest --test-dir engine-host/build --output-on-failure
```

The native program currently reports capabilities and validates that the host scaffold starts. It does not load `haloreach.dll`, parse proprietary map data, render the original menu, or provide multiplayer.

## Open-source contribution

Reach Community Client is licensed under GNU GPL-3.0-or-later. Contributions are reviewed through public pull requests. AI-assisted contributions are welcome when contributors understand, verify, test, and take responsibility for their code.

## Legal notice

Reach Community Client is an unofficial fan project and is not affiliated with Microsoft, Xbox, Bungie, 343 Industries, Halo Studios, or Valve. Halo, Halo: Reach and MCC are trademarks and/or copyrighted works of their respective owners. Users must own and install MCC/Reach independently. RCC does not bypass DRM, account restrictions, ownership checks, or anti-cheat protections.
