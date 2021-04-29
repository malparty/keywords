#!/bin/bash

set -e
run_cmd="dotnet run --project ./KeywordsApp/KeywordsApp.csproj --no-launch-profile --environment=Test --server.urls http://*:5000"

until /root/.dotnet/tools/dotnet-ef database update  --project ./KeywordsApp/KeywordsApp.csproj ; do
>&2 echo "PostgreSQL is starting up"
sleep 1
done

>&2 echo "PostgreSQL is up - executing command"
exec $run_cmd