#!/bin/sh

REPO=$(dirname $(readlink -f "$0"))
OUTDIR="$REPO/out"

tar czvf $OUTDIR/portable.tar.gz -C $OUTDIR/portable .
tar czvf $OUTDIR/win-x64.tar.gz -C $OUTDIR/win-x64 .
tar czvf $OUTDIR/win-arm64.tar.gz -C $OUTDIR/win-arm64 .
tar czvf $OUTDIR/linux-x64.tar.gz -C $OUTDIR/linux-x64 .
tar czvf $OUTDIR/linux-arm64.tar.gz -C $OUTDIR/linux-arm64 .
tar czvf $OUTDIR/osx-x64.tar.gz -C $OUTDIR/osx-x64 .
tar czvf $OUTDIR/osx-arm64.tar.gz -C $OUTDIR/osx-arm64 .

echo "Done! "
