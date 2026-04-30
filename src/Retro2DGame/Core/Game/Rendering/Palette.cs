using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class Palette
{
    private readonly Color[] _palette;
    
    public Palette()
    {
        _palette = new Color[256];
    }
    
    public Color this[int shade, int context]
    {
        get
        {
            return _palette[(shade << PaletteIndexBitmap.CONTEXT_LENGTH_BITS) | (context)];
        }
        set
        {
            _palette[(shade << PaletteIndexBitmap.CONTEXT_LENGTH_BITS) | (context)] = value;
        }
    }
}

