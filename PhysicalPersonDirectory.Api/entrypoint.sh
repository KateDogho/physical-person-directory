#!/bin/bash

# Start SQL Server
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
for i in {1..60}; do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -Q "SELECT 1;" > /dev/null 2>&1
    if [ $? -eq 0 ]; then
        break
    fi
    sleep 1
done

# Run your SQL scripts
for script in /usr/src/app/*.sql; do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i $script
done

# Keep the container running
tail -f /dev/null
