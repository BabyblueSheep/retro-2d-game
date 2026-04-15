using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class PaletteIndexBitmap
{
    public uint Width { get; }
    public uint Height { get; }

    private readonly byte[] _paletteIndexes;

    public PaletteIndexBitmap(uint width, uint height)
    {
        Width = width;
        Height = height;
        
        _paletteIndexes = new byte[width * height];
    }
}
