#!/bin/bash
CURRENT_DIR=$(cd $(dirname $0); pwd)

dotnet publish  ${CURRENT_DIR}/SampleGame.Desktop/SampleGame.Desktop.csproj -c Release -r osx-arm64 -p:PublishAot=true -o ./out/osx-arm64