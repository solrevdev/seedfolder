# Solrevdev.SeedFolder

[![GitHub last commit](https://img.shields.io/github/last-commit/solrevdev/seedfolder)](https://github.com/solrevdev/seedfolder) [![CI](https://github.com/solrevdev/seedfolder/workflows/CI/badge.svg)](https://github.com/solrevdev/seedfolder) [![Twitter Follow](https://img.shields.io/twitter/follow/solrevdev?label=Follow&style=social)](https://twitter.com/solrevdev)


```
                     _  __       _     _
  ___  ___  ___  __| |/ _| ___ | | __| | ___ _ __
 / __|/ _ \/ _ \/ _` | |_ / _ \| |/ _` |/ _ \ '__|
 \__ \  __/  __/ (_| |  _| (_) | | (_| |  __/ |
 |___/\___|\___|\__,_|_|  \___/|_|\__,_|\___|_|
```


## Overview

This is a .NET Core Global Tool that will create a folder named after either the first argument passed to it or if no
argument is passed it will ask you for the folder name. It will then copy some default standard dotfiles over.

For example:

```bash
seedfolder
dotnet run --project src/solrevdev.seedfolder.csproj --framework net8.0
▲   Do you want to prefix the folder with the date? [Y/n] y
▲   What do you want the folder to be named? temp
‍▲   Creating the directory 2020-12-10_temp
‍▲   Copying .dockerignore to 2020-12-10_temp/.dockerignore
‍▲   Copying .editorconfig to 2020-12-10_temp/.editorconfig
‍▲   Copying .gitattributes to 2020-12-10_temp/.gitattributes
‍▲   Copying .gitignore to 2020-12-10_temp/.gitignore
‍▲   Copying .prettierignore to 2020-12-10_temp/.prettierignore
‍▲   Copying .prettierrc to 2020-12-10_temp/.prettierrc
‍▲   Copying omnisharp.json to 2020-12-10_temp/omnisharp.json
▲   Done!

seedfolder
dotnet run --project src/solrevdev.seedfolder.csproj --framework net9.0
▲   Do you want to prefix the folder with the date? [Y/n] n
▲   What do you want the folder to be named? temp
‍▲   Creating the directory temp
‍▲   Copying .dockerignore to temp/.dockerignore
‍▲   Copying .editorconfig to temp/.editorconfig
‍▲   Copying .gitattributes to temp/.gitattributes
‍▲   Copying .gitignore to temp/.gitignore
‍▲   Copying .prettierignore to temp/.prettierignore
‍▲   Copying .prettierrc to temp/.prettierrc
‍▲   Copying omnisharp.json to temp/omnisharp.json
▲   Done!

seedfolder temp
dotnet run --project src/solrevdev.seedfolder.csproj --framework net8.0 temp
▲   Found 1 params to process.
‍▲   Creating the directory temp
‍▲   Copying .dockerignore to temp/.dockerignore
‍▲   Copying .editorconfig to temp/.editorconfig
‍▲   Copying .gitattributes to temp/.gitattributes
‍▲   Copying .gitignore to temp/.gitignore
‍▲   Copying .prettierignore to temp/.prettierignore
‍▲   Copying .prettierrc to temp/.prettierrc
‍▲   Copying omnisharp.json to temp/omnisharp.json
▲   Done!

```

It will also copy the following dotfiles from the `src/Data` folder over:

* .dockerignore
* .editorconfig
* .gitattributes
* .gitignore
* .prettierignore
* .prettierrc

## Requirements

This tool requires .NET 8.0 or .NET 9.0 SDK to be installed on your system.

## Installation

Locally without publishing it on NuGet

```powershell

dotnet pack
dotnet tool install --global --add-source ./nupkg solrevdev.seedfolder

```

Normally via NuGet

```powershell
dotnet tool install --global solrevdev.seedfolder
```

To uninstall

```powershell
dotnet tool uninstall -g solrevdev.seedfolder
```

## Publish to Nuget

Uses a GitHub secret to store a `NUGET_API_KEY` API Key created over at NuGet in order to build and deploy via GitHub actions to NuGet.

When you commit bump the version in the `csproj` file

```xml
<Version>1.0.1</Version>
```
