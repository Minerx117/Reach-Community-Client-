# RCC engine-host protocol (draft)

Version 0.01.03 establishes only the RCC-owned C++20 diagnostic host. It supports `--version`, `--self-check`, and `--capabilities`; it does not load or execute Halo: Reach.

## Intended integration

The Avalonia shell will eventually start RCC's own native host as a child process and exchange versioned JSON-line messages over standard input/output. Standard error is reserved for diagnostics. Messages will carry request IDs so concurrent status and lifecycle operations can be correlated.

## Planned commands

- `status` — report host version, supported game build, and capability states.
- `preflight` — perform read-only checks against a user-selected game installation.
- `start_main_menu` — initialize RCC's supported runtime and load the actual main-menu scene.
- `stop` — request a clean engine shutdown.

## Safety and compatibility rules

- Never accept arbitrary shell commands from the UI or server directory.
- Resolve game paths and validate expected files before attempting engine startup.
- Do not download, bundle, or redistribute Microsoft game files.
- Never weaken, bypass, or disable anti-cheat, DRM, Steam ownership, or account checks.
- Reject incompatible game builds rather than guessing at offsets or silently patching them.
- Log host and game build identifiers, but do not log credentials or personal account tokens.

The JSON-line request/response protocol is not implemented yet; see the capability report for the current host status.
