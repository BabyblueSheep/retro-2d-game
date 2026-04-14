using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class PaletteIndexBitmap
{
    private readonly byte[] _paletteIndexes;

    public PaletteIndexBitmap(uint width, uint height)
    {
        _paletteIndexes = new byte[width * height];
    }
}
