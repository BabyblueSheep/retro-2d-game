using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class PaletteIndexBitmap
{
    public const int SHADE_LENGTH_BITS = 5;
    public const int CONTEXT_LENGTH_BITS = 3;

    public const byte SHADE_BITS_MASK = (1 << SHADE_LENGTH_BITS) - 1;
    public const byte CONTEXT_BITS_MASK = ((1 << CONTEXT_LENGTH_BITS) - 1) << SHADE_LENGTH_BITS;

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

    public static PaletteIndexBitmap ReadFile(string path)
    {
        var file = SDL.IOFromFile(path, "rb");

        SDL.ReadU32BE(file, out var imageWidth);
        SDL.ReadU32BE(file, out var imageHeight);

        var bitmap = CreateEmpty(imageWidth, imageHeight);

        for (uint h = 0; h < imageHeight; h++)
        {
            for (uint w = 0; w < imageWidth; w++)
            {
                SDL.ReadU8(file, out var rawPaletteIndex);
                bitmap.WriteIndex(rawPaletteIndex, w, h);
            }
        }

        SDL.CloseIO(file);

        return bitmap;
    }

    public int ReadIndex(uint positionX, uint positionY)
    {
        return _paletteIndexes[positionX + positionY * Width];
    }

    public void WriteIndex(int index, uint positionX, uint positionY)
    {
        _paletteIndexes[positionX + positionY * Width] = (byte)index;
    }

    public int ReadShade(uint positionX, uint positionY)
    {
        return _paletteIndexes[positionX + positionY * Width] & SHADE_BITS_MASK;
    }

    public void WriteShade(int shade, uint positionX, uint positionY)
    {
        var shadeConverted = (byte)(shade & SHADE_BITS_MASK);

        _paletteIndexes[positionX + positionY * Width] &= 255 ^ SHADE_BITS_MASK;
        _paletteIndexes[positionX + positionY * Width] |= shadeConverted;
    }

    public int ReadContext(uint positionX, uint positionY)
    {
        return (_paletteIndexes[positionX + positionY * Width] & CONTEXT_BITS_MASK) >> SHADE_LENGTH_BITS;
    }

    public void WriteContext(int context, uint positionX, uint positionY)
    {
        var contextConverted = (byte)((context & CONTEXT_BITS_MASK) << SHADE_LENGTH_BITS);

        _paletteIndexes[positionX + positionY * Width] &= 255 ^ CONTEXT_BITS_MASK;
        _paletteIndexes[positionX + positionY * Width] |= contextConverted;
    }
}
