#!/bin/bash
MAX_WARNINGS=100

dotnet clean
count=$( ( dotnet build | grep warning | grep -v 'Migrations/20' | wc -l ) )
if [ $count -le $MAX_WARNINGS ]; then
  exit 0
fi

echo '[ERROR] Maximum warning limit exceed, Please fix warnings before push the commits'
exit -1;
