#!/bin/bash

set -e
run_cmd="dotnet run --server.urls http://*:5000 --launch-profile Test"

until /root/.dotnet/tools/dotnet-ef database update; do
>&2 echo "PostgreSQL is starting up"
sleep 1
done

>&2 echo "PostgreSQL is up - executing command"
exec $run_cmd