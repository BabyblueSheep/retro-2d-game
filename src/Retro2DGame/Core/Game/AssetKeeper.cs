using Retro2DGame.Core.Game.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Retro2DGame.Core.Game;

internal sealed class AssetKeeper
{
    private readonly PaletteIndexBitmap _defaultBitmap;
    private readonly Dictionary<string, PaletteIndexBitmap> _bitmaps;

    private readonly Dictionary<string, Rectangle> _bitmapFrames;

    public AssetKeeper()
    {
        _defaultBitmap = PaletteIndexBitmap.CreateEmpty(8, 8);
        _bitmaps = [];

        _bitmapFrames = [];
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

    public void AddFrame(string label, Rectangle frame)
    {
        _bitmapFrames[label] = frame;
    }

    public Rectangle RequestFrame(string label)
    {
        var bitmap = _bitmapFrames.GetValueOrDefault(label, Rectangle.Empty);
        return bitmap;
    }
}
