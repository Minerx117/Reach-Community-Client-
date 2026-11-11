# Contributing

Thanks for helping improve Reach Community Client. Contributions of code, tests, documentation, accessibility feedback, translations, and bug reports are welcome.

## Before starting

1. Search existing issues and pull requests before opening a new one.
2. For a substantial change, open an issue or discussion first so the community can agree on the direction.
3. Keep changes focused. Small pull requests are easier to review.

## Development setup

- Install the .NET 10 SDK.
- Install Git.
- Clone the repository.
- Run `dotnet restore ReachCommunityClient.sln`.
- Run `dotnet build ReachCommunityClient.sln`.
- Run `dotnet test ReachCommunityClient.sln`.
- Run the desktop app with `dotnet run --project src/ReachCommunityClient.App`.

## Pull requests

- Create a topic branch from `main`.
- Explain the problem and the chosen solution.
- Add tests for core behaviour where practical.
- Run build and tests before submitting.
- Do not bundle unrelated formatting or refactoring changes.
- Be respectful and constructive in review discussions.

## AI-assisted contributions

AI tools may be used to help write code, tests, or documentation. Contributors remain responsible for everything they submit: understand the changes, check licensing and provenance, test the result, and disclose material AI assistance when requested by maintainers. Never submit unreviewed generated code.

## Game files and intellectual property

Do not submit Halo, Halo: Reach, MCC, or other third-party copyrighted game files, extracted assets, keys, account credentials, or proprietary binaries. The project should work with a copy of MCC that the user has independently obtained through Steam. Do not use this repository to bypass DRM, anti-cheat, account restrictions, or platform protections.
