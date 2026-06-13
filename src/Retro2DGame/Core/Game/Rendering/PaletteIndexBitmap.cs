using Retro2DGame.Core.SDLWrappers;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Retro2DGame.Core.Game.Rendering;

internal unsafe sealed class PaletteIndexBitmap : IDisposable
{
    public const int SHADE_LENGTH_BITS = 5;
    public const int CONTEXT_LENGTH_BITS = 3;

    public const byte SHADE_BITS_MASK = (1 << SHADE_LENGTH_BITS) - 1;
    public const byte CONTEXT_BITS_MASK = ((1 << CONTEXT_LENGTH_BITS) - 1) << SHADE_LENGTH_BITS;
    public const byte CONTEXT_BITS_MASK_PRE_SHIFT = (1 << CONTEXT_LENGTH_BITS) - 1;

    public const int TRANSPARENCY_SHADE = 0;

    public int Width { get; }
    public int Height { get; }

    public int Size => Width * Height;

    internal readonly byte* _paletteIndices;

    public bool IsDisposed { get; private set; }

    private PaletteIndexBitmap(int width, int height)
    {
        Width = width;
        Height = height;
        
        _paletteIndices = (byte*)Marshal.AllocHGlobal(width * height * sizeof(byte));
        Clear();
    }

    public static PaletteIndexBitmap CreateEmpty(int width, int height)
    {
        var bitmap = new PaletteIndexBitmap(width, height);

        return bitmap;
    }

    public static PaletteIndexBitmap CreateFromFile(string path)
    {
        var file = SDL3.SDL.IOFromFile(path, "rb");

        SDL3.SDL.ReadU32BE(file, out var imageWidth);
        SDL3.SDL.ReadU32BE(file, out var imageHeight);

        var bitmap = CreateEmpty((int)imageWidth, (int)imageHeight);

        for (int h = 0; h < imageHeight; h++)
        {
            for (int w = 0; w < imageWidth; w++)
            {
                SDL3.SDL.ReadU8(file, out var rawPaletteIndex);
                bitmap.WriteIndex(rawPaletteIndex, w, h);
            }
        }

        SDL3.SDL.CloseIO(file);

        return bitmap;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadIndex(int positionX, int positionY)
    {
        return _paletteIndices[positionX + positionY * Width];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteIndex(int index, int positionX, int positionY)
    {
        _paletteIndices[positionX + positionY * Width] = (byte)index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadShade(int positionX, int positionY)
    {
        return _paletteIndices[positionX + positionY * Width] & SHADE_BITS_MASK;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteShade(int shade, int positionX, int positionY)
    {
        var shadeConverted = (byte)(shade & SHADE_BITS_MASK);

        _paletteIndices[positionX + positionY * Width] &= 255 ^ SHADE_BITS_MASK;
        _paletteIndices[positionX + positionY * Width] |= shadeConverted;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadContext(int positionX, int positionY)
    {
        return (_paletteIndices[positionX + positionY * Width] & CONTEXT_BITS_MASK) >> SHADE_LENGTH_BITS;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteContext(int context, int positionX, int positionY)
    {
        var contextConverted = (byte)((context & CONTEXT_BITS_MASK_PRE_SHIFT) << SHADE_LENGTH_BITS);

        _paletteIndices[positionX + positionY * Width] &= 255 ^ CONTEXT_BITS_MASK;
        _paletteIndices[positionX + positionY * Width] |= contextConverted;
    }

    public void Blit
    (
        PaletteIndexBitmap source,
        int destinationPositionX, int destinationPositionY,
        int sourcePositionX = 0, int sourcePositionY = 0,
        int width = 0, int height = 0,
        bool ignoreTransparency = true
    )
    {
        if (width <= 0)
            width = source.Width;
        if (height <= 0)
            height = source.Height;

        var minimumSamplePositionX = int.Max(0, sourcePositionX);
        var minimumSamplePositionY = int.Max(0, sourcePositionY);

        var maximumSamplePositionX = int.Min(source.Width, sourcePositionX + width);
        var maximumSamplePositionY = int.Min(source.Height, sourcePositionY + height);

        for (int y = minimumSamplePositionY; y < maximumSamplePositionY; y++)
        {
            var finalPositionY = destinationPositionY + (y - sourcePositionY);

            if (finalPositionY < 0 || finalPositionY >= Height)
                continue;

            for (int x = minimumSamplePositionX; x < maximumSamplePositionX; x++)
            {
                var finalPositionX = destinationPositionX + (x - sourcePositionX);

                if (finalPositionX < 0 || finalPositionX >= Width)
                    continue;

                var indexToCopy = source.ReadIndex(x, y);
                if (((indexToCopy & SHADE_BITS_MASK) == TRANSPARENCY_SHADE) && ignoreTransparency)
                    continue;

                WriteIndex(indexToCopy, finalPositionX, finalPositionY);
            }
        }
    }

    public void Blit
    (
        PaletteIndexBitmap source,
        int destinationPositionX, int destinationPositionY,
        Rectangle sourceRectangle,
        bool ignoreTransparency = true
    )
    {
        Blit(source, destinationPositionX, destinationPositionY, sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height, ignoreTransparency);
    }

    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _paletteIndices[x + y * Width] = 0;
            }    
        }
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            Marshal.FreeHGlobal((nint)_paletteIndices);

            IsDisposed = true;
        }
    }

    ~PaletteIndexBitmap()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

internal static class RendererBitmapExtension
{
    public static void BlitPaletteIndexBitmap
    (
        this Renderer renderer,
        PaletteIndexBitmap bitmap, Palette palette
    )
    {
        for (var h = 0; h < bitmap.Height; h++)
        {
            for (var w = 0; w < bitmap.Width; w++)
            {
                var index = bitmap.ReadIndex(w, h);

                renderer.SetDrawColor(palette[index]);
                renderer.RenderPoint(w, h);
            }
        }
    }

    public static unsafe void BlitPaletteIndexBitmap
    (
        this Surface surface, 
        PaletteIndexBitmap bitmap
    )
    {
        var pitchPixels = surface.Structure.Pitch / sizeof(uint);

        //var pixels = SDL.PointerToStructureArray<uint>(surface.Structure.Pixels, pitchPixels * surface.Structure.Height);
        //if (pixels == null)
        //    return;

        var pixels = new Span<uint>((uint*)surface.Structure.Pixels, pitchPixels * bitmap.Height);

        for (var h = 0; h < bitmap.Height; h++)
        {
            for (var w = 0; w < bitmap.Width; w++)
            {
                var index = bitmap.ReadIndex(w, h);
                pixels[w + h * pitchPixels] = surface.PaletteColors[index];
            }
        }

        //Marshal.Copy((int[])(object)pixels, 0, surface.Structure.Pixels, pixels.Length);

        //Marshal.StructureToPtr(surface.Structure, surface.Handle, true);
    }
}
