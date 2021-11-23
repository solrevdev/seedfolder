#!/usr/bin/env bash

cd $(dirname $0)

dotnet-format ../solrevdev.seedfolder.sln --fix-style warn
