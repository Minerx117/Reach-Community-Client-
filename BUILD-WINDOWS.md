# Windows build

## Reach Community Client

Requirements: .NET 10 SDK and Windows 10/11.

Run `BUILD-WINDOWS.cmd` from the repository root. The script restores the solution, runs automated tests, and publishes a self-contained Windows x64 launcher ZIP.

This build only creates RCC. It does not download, build, or run any external Halo client or backend. The current application performs read-only Steam/MCC/Reach diagnostics and can hand off to the official MCC Steam client when the user clicks the button.
