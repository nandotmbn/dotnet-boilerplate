#!/bin/bash

# Check if the correct number of arguments is provided
if [ "$#" -ne 1 ]; then
    echo "Usage: $0 <version>"
    exit 1
fi

# Arguments
VERSION="$1"

cd WebAPI
dotnet ef migrations add $VERSION
dotnet ef database update
sed -i.bak 's/"0": "Server=host.docker.internal;PORT=4810/"0": "Server=host.docker.internal;PORT=4811/g' appsettings.Development.json
dotnet ef database update
sed -i.bak 's/"0": "Server=host.docker.internal;PORT=4811/"0": "Server=host.docker.internal;PORT=4812/g' appsettings.Development.json
dotnet ef database update
sed -i.bak 's/"0": "Server=host.docker.internal;PORT=4812/"0": "Server=host.docker.internal;PORT=4813/g' appsettings.Development.json
dotnet ef database update
sed -i.bak 's/"0": "Server=host.docker.internal;PORT=4813/"0": "Server=host.docker.internal;PORT=4814/g' appsettings.Development.json
dotnet ef database update
sed -i.bak 's/"0": "Server=host.docker.internal;PORT=4814/"0": "Server=host.docker.internal;PORT=4810/g' appsettings.Development.json
rm appsettings.Development.json.bak
cd ..