using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class PaletteIndexBitmap
{
    public uint Width { get; }
    public uint Height { get; }

    private readonly byte[] _paletteIndexes;

    private PaletteIndexBitmap(uint width, uint height)
    {
        Width = width;
        Height = height;
        
        _paletteIndexes = new byte[width * height];
    }

    public static PaletteIndexBitmap CreateEmpty(uint width, uint height)
    {
        var bitmap = new PaletteIndexBitmap(width, height);

        return bitmap;
    }

    public static PaletteIndexBitmap ReadFile(string path, uint positionX, uint positionY, uint width, uint height)
    {
        var file = SDL.IOFromFile(path, "rb");

        SDL.ReadU32BE(file, out var imageWidth);
        SDL.ReadU32BE(file, out var imageHeight);

        SDL.LogInfo(SDL.LogCategory.Application, $"{imageWidth} {imageHeight}");

        for (uint h = 0; h < imageHeight; h++)
        {
            for (uint w = 0; w < imageWidth; w++)
            {
                SDL.ReadU8(file, out var paletteIndex);
            }
        }

        SDL.CloseIO(file);

        return CreateEmpty(2, 2);
    }
}
