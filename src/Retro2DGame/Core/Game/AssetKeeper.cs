using Retro2DGame.Core.Game.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game;

internal sealed class AssetKeeper
{
    private readonly Dictionary<string, PaletteIndexBitmap> _bitmaps;

    public AssetKeeper()
    {
        _bitmaps = [];
    }

    public void AddBitmap(string label, PaletteIndexBitmap bitmap)
    {
        _bitmaps[label] = bitmap;
    }
}
