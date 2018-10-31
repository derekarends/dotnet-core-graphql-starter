#!/usr/bin/env bash

if [ "$1" == "" ]; then
  echo evn missing
  exit 1
fi

if [ "$1" == "staging" ]; then
  echo publishing...
  dotnet publish -c Release
  rm ./bin/Release/netcoreapp2.0/publish/appsettings.Development.json
  rm ./bin/Release/netcoreapp2.0/publish/appsettings.json
elif [ "$1" == "prod" ]; then
  echo publishing...
  dotnet publish -c Release
  rm ./bin/Release/netcoreapp2.0/publish/appsettings.Development.json
  rm ./bin/Release/netcoreapp2.0/publish/appsettings.Staging.json
else
  echo incorrect env
  exit 1
fi

echo finished...

######## Notes #############
# export ASPNETCORE_ENVIRONMENT="Development/Staging/Production"
# docker run -d --name redis -p 6379:6379 --restart unless-stopped -v /tmp/redis/:/data redis redis-server /data
# docker run --name postgres -p 127.0.0.1:5432:5432 -e POSTGRES_PASSWORD=mysecretpassword -d postgres