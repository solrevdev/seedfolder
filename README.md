# Solrevdev.SeedFolder

```
                     _  __       _     _
  ___  ___  ___  __| |/ _| ___ | | __| | ___ _ __
 / __|/ _ \/ _ \/ _` | |_ / _ \| |/ _` |/ _ \ '__|
 \__ \  __/  __/ (_| |  _| (_) | | (_| |  __/ |
 |___/\___|\___|\__,_|_|  \___/|_|\__,_|\___|_|
```


## Overview

This is a .NET Core Global Tool that will create a folder named after either a Trello card reference or the current
Date in a YYYY-MM-DD format followed by the folder name and copy some default standard dotfiles over.

For example:


`MO-818_create-dotnet-tool`

`2020-09-29_create-dotnet-tool`

It will also copy the following dotfiles over:

* .dockerignore
* .editorconfig
* .gitattributes
* .gitignore
* .prettierignore
* .prettierrc

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
