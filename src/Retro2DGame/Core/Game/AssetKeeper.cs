using Retro2DGame.Core.Game.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game;

internal sealed class AssetKeeper
{
    private readonly PaletteIndexBitmap _defaultBitmap;
    private readonly Dictionary<string, PaletteIndexBitmap> _bitmaps;

    public AssetKeeper()
    {
        _defaultBitmap = PaletteIndexBitmap.CreateEmpty(8, 8);
        _bitmaps = [];
    }

    public void AddBitmap(string label, PaletteIndexBitmap asset)
    {
        _bitmaps[label] = asset;
    }

    public PaletteIndexBitmap RequestBitmap(string label)
    {
        var bitmap = _bitmaps.GetValueOrDefault(label, _defaultBitmap);
        return bitmap;
    }
}
