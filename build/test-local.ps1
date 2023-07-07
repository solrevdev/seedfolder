#!/usr/bin/env pwsh

$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$scriptDir = Split-Path -Path $scriptDir -Parent

Set-Location -Path $scriptDir

# Clean and build the project
dotnet clean .\solrevdev.seedfolder.sln
dotnet restore .\solrevdev.seedfolder.sln
dotnet build .\solrevdev.seedfolder.sln

# Create an artifact folder
$null = New-Item -Path .\artifacts -ItemType Directory -Force

# Remove any old nupkg files
Remove-Item -Path .\artifacts\nupkg\* -Recurse -Force

# Package the dotnet tool
dotnet pack .\solrevdev.seedfolder.sln -c Release -o .\artifacts\nupkg

# Uninstall any version you currently have installed
dotnet tool uninstall -g solrevdev.seedfolder

# Install the version we have just built and packaged
dotnet tool install -g --add-source .\artifacts\nupkg solrevdev.seedfolder

# Run the tool
seedfolder --help

# Now uninstall the tool ready for the next run
dotnet tool uninstall -g solrevdev.seedfolder

# And install it again from nuget
dotnet tool install -g solrevdev.seedfolder
