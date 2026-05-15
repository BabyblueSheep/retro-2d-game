using Retro2DGame.Core.SDL3;
using SDL3;
using System.Drawing;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class PaletteIndexBitmap
{
    public const int SHADE_LENGTH_BITS = 5;
    public const int CONTEXT_LENGTH_BITS = 3;

    public const byte SHADE_BITS_MASK = (1 << SHADE_LENGTH_BITS) - 1;
    public const byte CONTEXT_BITS_MASK = ((1 << CONTEXT_LENGTH_BITS) - 1) << SHADE_LENGTH_BITS;

    public const int TRANSPARENCY_SHADE = 0;

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

    public static PaletteIndexBitmap CreateFromFile(string path)
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

    public void Blit
    (
        PaletteIndexBitmap source,
        uint destinationPositionX, uint destinationPositionY,
        uint sourcePositionX = 0, uint sourcePositionY = 0,
        uint width = 0, uint height = 0,
        bool ignoreTransparency = true
    )
    {
        if (width == 0)
            width = source.Width;
        if (height == 0)
            height = source.Height;
        for (uint h = 0; h < height; h++)
        {
            for (uint w = 0; w < width; w++)
            {
                var samplePositionX = sourcePositionX + w;
                var samplePositionY = sourcePositionY + h;

                if (samplePositionX < 0 || samplePositionX >= source.Width)
                    continue;
                if (samplePositionY < 0 || samplePositionY >= source.Height)
                    continue;

                var indexToCopy = source.ReadIndex(samplePositionX, samplePositionY);
                if (((indexToCopy & SHADE_BITS_MASK) == TRANSPARENCY_SHADE) && ignoreTransparency)
                    continue;

                var finalPositionX = destinationPositionX + w;
                var finalPositionY = destinationPositionY + h;

                if (finalPositionX < 0 || finalPositionX >= Width)
                    continue;
                if (finalPositionY < 0 || finalPositionY >= Height)
                    continue;

                WriteIndex(indexToCopy, finalPositionX, finalPositionY);
            }
        }
    }

    public void Blit
    (
        PaletteIndexBitmap source,
        uint destinationPositionX, uint destinationPositionY,
        Rectangle sourceRectangle,
        bool ignoreTransparency = true
    )
    {
        Blit(source, destinationPositionX, destinationPositionY, (uint)sourceRectangle.X, (uint)sourceRectangle.Y, (uint)sourceRectangle.Width, (uint)sourceRectangle.Height, ignoreTransparency);
    }

    public void Clear()
    {
        Array.Clear(_paletteIndexes, 0, _paletteIndexes.Length);
    }
}

internal static class RendererBitmapExtension
{
    public static void BlitPaletteIndexBitmap
    (
        this Renderer renderer, PaletteIndexBitmap bitmap,
        int destinationX, int destinationY,
        Palette palette,
        bool ignoreTransparency = true
    )
    {
        for (uint h = 0; h < bitmap.Height; h++)
        {
            for (uint w = 0; w < bitmap.Width; w++)
            {
                var shade = bitmap.ReadShade(w, h);
                if (shade == PaletteIndexBitmap.TRANSPARENCY_SHADE && ignoreTransparency)
                    continue;

                var context = bitmap.ReadContext(w, h);

                renderer.SetDrawColor(palette[shade, context]);
                renderer.RenderPoint((int)(destinationX + w), (int)(destinationY + h));
            }
        }
    }
}
