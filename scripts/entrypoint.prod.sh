#!/bin/bash

set -e

# Import file_env from shared.sh
. shared.sh

file_env "ConnectionStrings__SELearning"
file_env "ASPNETCORE_Kestrel__Certificates__Default__Password"
file_env "ConnectionStrings__ProductionConnectionString"

exec dotnet SELearning.API.dll
