# AI Coding Agent Instructions for SeedFolder

## Project Overview
This is a .NET Global Tool that creates project directories and seeds them with standard dotfiles. The entire application is contained in a single file (`src/Program.cs`) for simplicity.

## Architecture Patterns

### Single-File Console Application
- All logic exists in `src/Program.cs` - no complex project structure
- Uses `McMaster.Extensions.CommandLineUtils` for CLI interactions, `Figgle`/`Figgle.Fonts` for ASCII headers, `Console.ForegroundColor` for output formatting
- Command-line parsing is handled manually in `Main()` - check for help flags before processing folder names

### Embedded Resources Pattern
Template files in `src/Data/` are embedded as resources, not file system copies:
```xml
<EmbeddedResource Include="Data/dockerignore" />
<EmbeddedResource Include="Data/editorconfig" />
```
Access via `Assembly.GetManifestResourceStream("solrevdev.seedfolder.Data.{filename}")` in `WriteFileAsync()`

### Multi-Framework Targeting
Targets `net10.0;net11.0`, the supported .NET versions. `global.json` pins SDK `10.0.100` with `rollForward: latestMajor`, so .NET 10 (LTS) is the baseline and the newest installed SDK is used. With SDK 11 installed the project builds `net10.0` and `net11.0`. With only SDK 10 it builds `net10.0` alone, because `TargetFrameworks` adds `net11.0` only when `NETCoreSdkVersion` is 11.0 or later. CI installs both SDKs, so the published package always holds both targets. .NET 8 and .NET 9 left support on 10 November 2026 and are no longer targeted. `RollForward` is `Major`, so the tool also runs on newer runtimes. Version bumps in `.csproj` trigger NuGet deployment on push to `master`.

## Key Development Workflows

### Local Testing Cycle
```bash
# Build and test locally (use build/test-local.sh script)
dotnet pack -c release -o artifacts/nupkg
dotnet tool uninstall -g solrevdev.seedfolder
dotnet tool install -g --add-source artifacts/nupkg solrevdev.seedfolder
```

### Version Management
- Bump `<Version>` in `src/solrevdev.seedfolder.csproj` to trigger CI/CD
- CI only runs on pushes to `master` branch (exclude commits with `***NO_CI***`, `[ci skip]`, `[skip ci]`)
- GitHub Actions automatically publishes to NuGet on master pushes

### Running During Development
```bash
# Interactive mode
dotnet run --project src/solrevdev.seedfolder.csproj

# With folder name argument
dotnet run --project src/solrevdev.seedfolder.csproj myfolder
```

## Project-Specific Conventions

### Input Sanitization
Always use `RemoveSpaces()` and `SafeNameForFileSystem()` for folder names:
```csharp
folderName = RemoveSpaces(folderName);      // Spaces → dashes
folderName = SafeNameForFileSystem(folderName);  // Invalid chars → dashes
```

### Date Prefixing Logic
Interactive mode offers date prefixing: `YYYY-MM-DD_foldername` format using `StringBuilder` for performance.

### Error Handling Pattern
- Check if folder exists before creation - exit with colored error message
- Use `Directory.Exists()` check, then `ConsoleColor.DarkRed` for error output
- No exceptions for user errors - graceful CLI messaging

## File Structure Details
- `src/Data/` contains template files without leading dots (e.g., `gitignore` not `.gitignore`)
- Files are copied with proper dotfile names during `WriteFileAsync()`
- `omnisharp.json` is copied without dot prefix (special case)

## CI/CD Integration Points
- GitHub Actions workflow in `.github/workflows/ci.yml` handles build → pack → publish
- Publishes with NuGet trusted publishing (OIDC) via `NuGet/login@v1`; no `NUGET_API_KEY` secret
- Pushes the package with the `dotnet nuget push --skip-duplicate` CLI, not a third-party action
- Build scripts in `build/` folder provide cross-platform local testing
