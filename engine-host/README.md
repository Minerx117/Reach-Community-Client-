# RCC native engine-host scaffold

This is Reach Community Client's own C++20 process scaffold. In v0.01.03 it is diagnostic-only: it reports the current implementation status and verifies that the host process can start. It does not load `haloreach.dll`, proprietary map files, the Halo engine, or any game asset.

## Build

```bash
cmake -S . -B build -DCMAKE_BUILD_TYPE=Release
cmake --build build --config Release
ctest --test-dir build -C Release --output-on-failure
```

## Commands

```text
rcc-engine-host --version
rcc-engine-host --self-check
rcc-engine-host --capabilities
```

The native runtime, renderer, original menu, and multiplayer integrations are future RCC implementations. No external Halo client is downloaded, built, or launched.
