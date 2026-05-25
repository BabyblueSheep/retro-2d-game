using Retro2DGame.Core.SDL3;
using SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;

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
            return _palette[(context << PaletteIndexBitmap.SHADE_LENGTH_BITS) | (shade)];
        }
        set
        {
            _palette[(context << PaletteIndexBitmap.SHADE_LENGTH_BITS) | (shade)] = value;
        }
    }

    public Color this[int index]
    {
        get
        {
            return _palette[index];
        }
        set
        {
            _palette[index] = value;
        }
    }
}

internal static class SurfacePaletteExtensions
{
    extension(Surface surface)
    {
        public void UpdatePalette(Palette palette)
        {
            for (int i = 0; i < 256; i++)
            {
                surface.PaletteColors[i] = SDL.MapSurfaceRGBA(surface.Handle, palette[i].R, palette[i].G, palette[i].B, palette[i].A);
            }
        }
    }
}