# RCC standalone engine and multiplayer roadmap

## Product target

Build Reach Community Client as an independent application. All RCC application, runtime, rendering, server-directory, multiplayer, and dedicated-server components are to be authored and maintained in this repository under its GPL-3.0-or-later licence.

## Development stages

### 1. Installation and compatibility

- Detect Steam/MCC/Reach content and record exact file metadata.
- Establish a supported-game-build manifest based on tested, user-owned installations.
- Keep diagnostics read-only and avoid guessing compatibility from filenames alone.

### 2. RCC native runtime research and proof of concept

- Document what engine interfaces are legitimately available to a third-party client.
- Evaluate whether Reach can be hosted through a documented/authorized interface or whether a clean-room compatibility layer is technically feasible.
- Build an RCC-owned process and window lifecycle, crash reporting, version handshake, and local IPC.
- Do not call a file-presence check an engine boot.

### 3. Original menu and rendering

- Establish a tested runtime initialization path for the supported Reach build.
- Load the user's own main-menu data through that runtime path.
- Render the actual menu scene and accept input; a static recreation or screenshot does not meet acceptance.

### 4. Community browser and multiplayer

- Specify and implement an RCC-owned server directory API with real health/heartbeat reporting.
- Design lobby/session, join/leave, authentication, NAT/connectivity, and server discovery.
- Implement a transport and game-state synchronization layer that is genuinely compatible with the chosen game runtime.
- Test at least two independent clients joining and exchanging real in-game traffic.

### 5. Dedicated servers and portability

- Investigate headless runtime feasibility and administration.
- Add Windows, Linux, and macOS support only after separate builds and in-game validation.

## Current status in v0.01.03

- Steam/MCC/Reach file diagnostics: implemented, not yet validated against every MCC build.
- RCC native host: diagnostic scaffold only.
- Original menu renderer/runtime: not implemented.
- Live server directory: not implemented.
- Lobby, host/join, transport, game-state synchronization: not implemented.
- Dedicated server: not implemented.

## Acceptance criteria

- **Installation detection:** report file paths and build identifiers; missing DLC is actionable.
- **Engine startup:** initialize the RCC-owned supported runtime and report exact engine version.
- **Original main menu:** show the real main-menu scene from the user's own installation and accept input.
- **Server browser:** display live directory results, with stale/unreachable servers clearly marked.
- **Multiplayer:** two independent installations successfully host/join and exchange actual in-game traffic.
- **Dedicated server:** an RCC-owned headless server boots, is discoverable, and accepts clients.
- **Cross-platform:** each operating system is built and tested separately before being called supported.
