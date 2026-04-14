using SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Retro2DGame.Core.SDL3.Extensions;

internal static class SDLExtensions
{
    public static SDL.FColor ToFColor(this Color color)
    {
        return new SDL.FColor(color.R / (float)byte.MaxValue, color.G / (float)byte.MaxValue, color.B / (float)byte.MaxValue, color.A / (float)byte.MaxValue);
    }
}
