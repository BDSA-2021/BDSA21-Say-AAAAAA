#!/bin/bash

set -e

# Import file_env from shared.sh
. shared.sh

file_env "ConnectionStrings__SELearning"
file_env "ConnectionStrings__ProductionConnectionString"
file_env "ASPNETCORE_Kestrel__Certificates__Default__Password"

echo "urls: $ASPNETCORE_URLS"

exec dotnet run --project SELearning.API --no-launch-profile
