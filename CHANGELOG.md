# Changelog

## 0.01.03

- Added a Reach installation compatibility report covering `haloreach.dll`, Reach maps, `mainmenu.map`, MCC launch executables, Steam build ID, and Reach DLL file version.
- Added detection for an optional Project Noble standalone backend.
- Added a documented Windows build script for the separate MIT-licensed Project Noble source.
- Added a launcher action that starts the Project Noble client with its documented original-main-menu arguments when game files and backend are detected.
- Documented that the Project Noble backend provides its own live server browser and multiplayer/session features; RCC does not fake or claim to implement Reach's proprietary game networking.
- Added explicit diagnostics and installation guidance for missing Reach content.

## 0.01.01

- Restored the Avalonia application and Core source directories.
- Added Steam MCC installation discovery and a C++20 engine-host diagnostic scaffold.
- Defined initial engine and multiplayer capability contracts.
