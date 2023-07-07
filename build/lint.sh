#!/usr/bin/env bash

# shellcheck disable=SC2086,SC2164,SC2046
cd $(dirname $0)

dotnet-format ../solrevdev.seedfolder.sln --fix-style warn
