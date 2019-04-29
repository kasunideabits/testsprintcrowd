#!/bin/bash
MAX_WARNINGS=50

dotnet clean
count=$( ( dotnet build | grep warning | wc -l ) )
if [ $count -ge $MAX_WARNINGS ]; then
   echo '[ERROR] Maximum warning limit exceed, Please fix warnings before push the commits'
   exit -1;
fi
exit 0
