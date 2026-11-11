# Architecture

## Current projects

- `ReachCommunityClient.App`: Avalonia desktop UI for Windows, macOS, and Linux.
- `ReachCommunityClient.Core`: Steam/MCC discovery, read-only compatibility diagnostics, capability model, and RCC-owned multiplayer contracts.
- `ReachCommunityClient.Core.Tests`: unit tests for core behaviour and capability status.
- `engine-host`: RCC-owned C++20/CMake diagnostic host; no Reach engine loading or rendering yet.

## Target RCC architecture

The Avalonia shell is responsible for installation discovery, configuration, diagnostics, and UI. RCC's own native engine host will eventually own game-runtime lifecycle, renderer/window integration, input, resource loading, and scene/menu presentation. A separate RCC service layer will own server-directory communication, lobby/session coordination, and dedicated-server administration. Actual gameplay still requires a compatible game-runtime integration and an implemented game networking layer.

The UI and native runtime are intended to communicate through a versioned, local IPC protocol. The community server directory is a metadata service, not the game transport; a live server browser alone is not multiplayer.

## Current launch flow

1. Discover Steam library roots.
2. Read each Steam root's `steamapps/libraryfolders.vdf` for additional libraries.
3. Find MCC Steam App ID `976730` and its app manifest.
4. Display read-only Reach content diagnostics.
5. Only when the user presses the button, ask Steam to launch official MCC.

RCC does not directly start the original Reach menu in this release.

## RCC development principles

- All RCC application, engine-host, runtime, and multiplayer implementation work belongs in this repository.
- Keep the native runtime and cross-platform interface separated behind documented, versioned interfaces.
- Use reproducible tests and compatibility reports for every supported game build.
- Do not mark a capability as implemented until it has been tested against a real user-owned installation.

## Safety and compatibility constraints

- Read-only inspection of the user's local game installation unless a future feature documents an operation and obtains explicit consent.
- No game binaries, maps, textures, sounds, or proprietary assets in the repository or releases.
- No DRM, anti-cheat, Steam ownership, or account-protection bypass.
- Engine adapters must be version-specific and fail closed on unknown game builds.
- No claim of menu or multiplayer functionality until verified against a legitimate local installation and real multi-client test.
