#!/usr/bin/env bash

cd $(dirname $0)

dotnet-outdated ../solrevdev.seedfolder.sln -u:prompt
