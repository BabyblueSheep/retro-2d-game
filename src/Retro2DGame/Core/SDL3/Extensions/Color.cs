using SDL3;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Game.Core.SDL3.Extensions;

internal static class ColorExtensions
{
    public static SDL.SDL_FColor ToFColor(this Color color)
    {
        return new SDL.SDL_FColor()
        {
            r = color.R / byte.MaxValue,
            g = color.G / byte.MaxValue,
            b = color.B / byte.MaxValue,
            a = color.A / byte.MaxValue
        };
    }
}
