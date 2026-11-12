# Repository Agent Guide

This file documents the project-specific guidance for coding agents working in this repository.

## Project overview

`seedfolder` is a .NET Global Tool that creates project directories from embedded templates. The application is a single-file console program in `src/Program.cs` and targets `net10.0` and `net11.0`. Only supported .NET versions are targeted; .NET 8 and .NET 9 left support on 10 November 2026.

## SDK policy

`global.json` pins SDK `10.0.100` with `rollForward: latestMajor`, so .NET 10 (LTS) is the baseline and the newest installed SDK is used. With SDK 11 installed the project builds `net10.0` and `net11.0`. With only SDK 10 it builds `net10.0` alone, because `TargetFrameworks` adds `net11.0` only when `NETCoreSdkVersion` is 11.0 or later. CI installs both SDKs, so the published package always holds both targets. Do not drop the `TargetFrameworks` condition or the explicit `10.0.x` and `11.0.x` entries in `ci.yml`.

## Required validation

Run the same Release-mode sequence used by CI:

```bash
dotnet build solrevdev.seedfolder.sln --configuration Release
./tests/integration-test.sh net10.0
./tests/integration-test.sh net11.0  # needs SDK 11
dotnet pack solrevdev.seedfolder.sln -c Release --no-build --include-source --include-symbols
```

Packages are written to `src/nupkg/`. For interactive local install/uninstall testing, see `build/test-local.sh` and review its global-tool side effects before running it.

Run the tool directly with:

```bash
dotnet run --project src/solrevdev.seedfolder.csproj
dotnet run --project src/solrevdev.seedfolder.csproj -- myfolder
dotnet run --project src/solrevdev.seedfolder.csproj -- --dry-run -t node myfolder
```

## Architecture

- `src/Program.cs` contains CLI parsing, interactive prompts, validation, rendering, and template extraction.
- `McMaster.Extensions.CommandLineUtils` supplies prompt helpers; option parsing is implemented in `Main()` and accepts options before or after the single folder-name argument.
- `Figgle` and `Figgle.Fonts` render the ASCII header; colored output uses `Console.ForegroundColor` through the local `WriteLine()` helper.
- `src/Data/` files are embedded by `src/solrevdev.seedfolder.csproj` and extracted with `WriteFileAsync()`.
- `TemplateFile` and `ProjectType` define the six templates: markdown, dotnet, node, python, ruby, and universal.
- Folder names pass through `RemoveSpaces()` and `SafeNameForFileSystem()` before use.

## Release model

- `<Version>` in `src/solrevdev.seedfolder.csproj` is the release source of truth and is bumped manually.
- Pull requests and `release/*` pushes build, test, pack, and verify the exact versioned package; only a push to `master` publishes to NuGet.
- A publish-capable change must use a version that is not already associated with another commit. After NuGet publication succeeds, CI creates the lightweight `v<Version>` tag for that exact master commit.
- Release retries are accepted only when the existing tag resolves to the same commit and its csproj contains the same version.
- Keep `actions/checkout@v5` and `actions/setup-dotnet@v5` unless a deliberate, validated action-major upgrade is required; both use the Node 24 action runtime.
- NuGet publication uses native `dotnet nuget push --skip-duplicate` with a short-lived key from NuGet trusted publishing (`NuGet/login@v1`); the publish job needs `id-token: write` and no `NUGET_API_KEY` secret.
- Push commits containing `***NO_CI***`, `[ci skip]`, or `[skip ci]` skip the build job. Pull-request validation is not bypassed by these markers.

## Commit messages

Use Conventional Commits with an imperative subject, for example `ci: harden release workflow`. Add a scope when useful and mark breaking changes with `!` and/or a `BREAKING CHANGE:` footer.

## Global versus repository guidance

Keep machine-specific operational rules—Python environment management, browser tooling, SVG rasterization, safe deletion utilities, and Homebrew/.NET SDK repair—in global agent settings. They are useful defaults but are not requirements of this .NET project.
