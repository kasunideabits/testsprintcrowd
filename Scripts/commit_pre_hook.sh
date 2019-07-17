#!/bin/bash
MAX_WARNINGS=150

dotnet clean
count=$( ( dotnet build | grep Warning | tr -dc '0-9') )
if [ $count -le $MAX_WARNINGS ]; then
  exit 0
fi

echo "[ERROR] Maximum warning limit exceed $count, Please fix warnings before push the commits"
exit -1;
