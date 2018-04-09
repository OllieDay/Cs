#!/bin/bash

if [ "$(uname)" == "Darwin" ]; then
	runtime=osx-x64
elif [ "$(expr substr $(uname -s) 1 5)" == "Linux" ]; then
	runtime=linux-x64
fi

output="/usr/local/Cs"

dotnet build Cs/Cs.csproj -o $output -c Release -r $runtime
chmod +x $output/Cs
ln -s $output/Cs /usr/local/bin/cs
