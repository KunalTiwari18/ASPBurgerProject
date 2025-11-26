#!/bin/sh
# entrypoint.sh - ensure the app binds to the PORT Render provides
: "${PORT:=8080}"
export ASPNETCORE_URLS="http://+:${PORT}"
echo "Starting BBURGERClone on ${ASPNETCORE_URLS}"
exec dotnet BBURGERClone.dll
