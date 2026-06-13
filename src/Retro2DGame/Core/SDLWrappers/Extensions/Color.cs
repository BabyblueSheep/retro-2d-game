using System.Drawing;

namespace Retro2DGame.Core.SDLWrappers.Extensions;

internal static class SDLExtensions
{
    public static SDL3.SDL.FColor ToFColor(this Color color)
    {
        return new SDL3.SDL.FColor(color.R / (float)byte.MaxValue, color.G / (float)byte.MaxValue, color.B / (float)byte.MaxValue, color.A / (float)byte.MaxValue);
    }
}
