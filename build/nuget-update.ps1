#!/usr/bin/env pwsh

$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$scriptDir = Split-Path -Path $scriptDir -Parent

Set-Location -Path $scriptDir

dotnet-outdated ./solrevdev.seedfolder.sln -u:prompt
