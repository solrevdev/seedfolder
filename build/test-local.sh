#!/usr/bin/env bash

# shellcheck disable=SC2046
cd $(dirname "$0") || exit

# clean and build the project
dotnet clean ../solrevdev.seedfolder.sln
dotnet restore ../solrevdev.seedfolder.sln
dotnet build ../solrevdev.seedfolder.sln

# create an artifact folder
mkdir -p ../artifacts

# remove any old nupkg files
rm -rf ../artifacts/nupkg

# package the dotnet tool
dotnet pack ../solrevdev.seedfolder.sln -c release -o ../artifacts/nupkg

# uninstall any version you currently have installed
dotnet tool uninstall -g solrevdev.seedfolder

# install the version we have just built and packaged
dotnet tool install -g --add-source ../artifacts/nupkg solrevdev.seedfolder

# run the tool
seedfolder --help

# now uninstall the tool ready for the next run
dotnet tool uninstall -g solrevdev.seedfolder

# and install it again from nuget
dotnet tool install -g solrevdev.seedfolder
