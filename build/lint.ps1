#!/usr/bin/env pwsh

$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$scriptDir = Split-Path -Path $scriptDir -Parent

Set-Location -Path $scriptDir

dotnet-format ./solrevdev.seedfolder.sln --fix-style warn
