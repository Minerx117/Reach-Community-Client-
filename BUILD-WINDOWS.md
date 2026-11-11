# Windows build

## Reach Community Client

Requirements: .NET 10 SDK and Windows 10/11.

Run `BUILD-WINDOWS.cmd` from the repository root. The script restores, runs tests, and publishes a self-contained Windows x64 launcher package.

## Optional standalone Reach backend

The desktop shell can launch Project Noble as an optional native backend. Install Git, CMake, and Visual Studio with the C++ desktop workload, then run `BUILD-PROJECT-NOBLE-BACKEND.cmd`. See `docs/PROJECT-NOBLE-BACKEND.md` for the backend's separate MIT licence, required user-owned MCC/Reach files, and known platform limitations.

The two builds are separate: the RCC app build does not automatically download or bundle Project Noble, MCC, Halo: Reach, maps, or other copyrighted game content.
