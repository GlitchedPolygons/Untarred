#!/bin/sh

REPO=$(dirname $(readlink -f "$0"))
OUTDIR="$REPO/out"
SRCDIR="$REPO/Untarred"

if [ -d "$OUTDIR" ]; then
	rm -rf $OUTDIR
fi

mkdir -p $OUTDIR/portable
mkdir -p $OUTDIR/win-x64
mkdir -p $OUTDIR/win-arm64
mkdir -p $OUTDIR/linux-x64
mkdir -p $OUTDIR/linux-arm64
mkdir -p $OUTDIR/osx-x64
mkdir -p $OUTDIR/osx-arm64

dotnet publish $SRCDIR/Untarred.csproj -c Release -o $OUTDIR/portable

dotnet publish $SRCDIR/Untarred.csproj -r win-x64 -p:PublishTrimmed=true -p:PublishSingleFile=true --self-contained true -c Release -o $OUTDIR/win-x64

cp $SRCDIR/Untarred.ico $OUTDIR/win-x64/
cp $SRCDIR/Untarred.reg $OUTDIR/win-x64/
cp $SRCDIR/Install.bat $OUTDIR/win-x64/

dotnet publish $SRCDIR/Untarred.csproj -r win-arm64 -p:PublishTrimmed=true -p:PublishSingleFile=true --self-contained true -c Release -o $OUTDIR/win-arm64

cp $SRCDIR/Untarred.ico $OUTDIR/win-arm64/
cp $SRCDIR/Untarred.reg $OUTDIR/win-arm64/
cp $SRCDIR/Install.bat $OUTDIR/win-arm64/

dotnet publish $SRCDIR/Untarred.csproj -r linux-x64 -p:PublishTrimmed=true -p:PublishSingleFile=true --self-contained true -c Release -o $OUTDIR/linux-x64

dotnet publish $SRCDIR/Untarred.csproj -r linux-arm64 -p:PublishTrimmed=true -p:PublishSingleFile=true --self-contained true -c Release -o $OUTDIR/linux-arm64

dotnet publish $SRCDIR/Untarred.csproj -r osx-x64 --p:PublishTrimmed=true -p:PublishSingleFile=true --self-contained true -c Release -o $OUTDIR/osx-x64

dotnet publish $SRCDIR/Untarred.csproj -r osx-arm64 -p:PublishTrimmed=true -p:PublishSingleFile=true --self-contained true -c Release -o $OUTDIR/osx-arm64

echo "Done! "
