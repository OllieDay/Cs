#!/bin/sh

if [ "$(uname)" == "Darwin" ]; then
	runtime=osx-x64
elif [ "$(expr substr $(uname -s) 1 5)" == "Linux" ]; then
	runtime=linux-x64
fi

dotnet build Cs/Cs.csproj -o /usr/local/Cs -c Release -r $runtime
ln -s /usr/local/Cs/Cs /usr/local/bin/cs
