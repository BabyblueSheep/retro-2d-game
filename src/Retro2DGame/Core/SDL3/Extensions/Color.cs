using SDL3;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Game.Core.SDL3.Extensions;

internal static class ColorExtensions
{
    public static SDL.FColor ToFColor(this Color color)
    {
        return new SDL.FColor(color.R / byte.MaxValue, color.G / byte.MaxValue, color.B / byte.MaxValue, color.A / byte.MaxValue);
    }
}
