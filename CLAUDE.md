# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET Core Global Tool called `seedfolder` that creates directories and populates them with standard dotfiles. The tool is designed to quickly scaffold project folders with common configuration files.

## Development Commands

### Build and Test
```bash
# Build the solution
dotnet build solrevdev.seedfolder.sln --configuration Release

# Pack for local installation
dotnet pack

# Install locally for testing
dotnet tool install --global --add-source ./nupkg solrevdev.seedfolder

# Uninstall after testing
dotnet tool uninstall -g solrevdev.seedfolder
```

### Run the Tool During Development
```bash
# Run with interactive prompts
dotnet run --project src/solrevdev.seedfolder.csproj

# Run with folder name argument
dotnet run --project src/solrevdev.seedfolder.csproj myfolder
```

## Architecture

### Single File Structure
The entire application is contained in `src/Program.cs` - a single-file console application using:
- **McMaster.Extensions.CommandLineUtils** for command-line parsing and prompts
- **Figgle** for ASCII art header generation
- **Colorful.Console** for colored console output

### Embedded Resources
Template files are stored as embedded resources in `src/Data/` and copied to new folders:
- `dockerignore` → `.dockerignore`
- `editorconfig` → `.editorconfig` 
- `gitattributes` → `.gitattributes`
- `gitignore` → `.gitignore`
- `prettierignore` → `.prettierignore`
- `prettierrc` → `.prettierrc`
- `omnisharp.json` → `omnisharp.json`

### Key Methods
- `WriteFileAsync()` - Extracts embedded resources and writes them to the target directory
- `RemoveSpaces()` - Sanitizes folder names by replacing spaces with dashes
- `SafeNameForFileSystem()` - Removes invalid filesystem characters from folder names

## Multi-Target Framework Support
The project targets .NET 8.0 (LTS) and 9.0 (STS) to support current and modern .NET versions. .NET 8 provides long-term support until November 2026, while .NET 9 offers the latest features with 18-month support.

## CI/CD
GitHub Actions workflow builds, packs, and publishes to NuGet on pushes to master branch. Version is controlled by the `<Version>` property in the .csproj file.