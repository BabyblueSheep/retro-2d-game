using SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text;

namespace Retro2DGame.Core.SDL3;

internal sealed class Surface : IDisposable
{
    public int Width
    {
        get
        {
            var structure = SDL.PointerToStructure<SDL.Surface>(Handle);
            if (structure == null)
                return 0;
            return structure.Value.Width;
        }
    }

    public int Height
    {
        get
        {
            var structure = SDL.PointerToStructure<SDL.Surface>(Handle);
            if (structure == null)
                return 0;
            return structure.Value.Height;
        }
    }

    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private Surface(nint handle)
    {
        Handle = handle;
    }

    public static Surface Create(int width, int height, SDL.PixelFormat pixelFormat)
    {
        var handle = SDL.CreateSurface(width, height, pixelFormat);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create surface: {SDL.GetError()}");
        }

        var surface = new Surface(handle);
        return surface;
    }

    public static Surface GetFromWindow(Window window)
    {
        var handle = SDL.GetWindowSurface(window.Handle);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create surface: {SDL.GetError()}");
        }

        var surface = new Surface(handle);
        return surface;
    }

    public static Surface LoadPNG(string filepath)
    {
        var handle = SDL.LoadPNG(filepath);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't load PNG: {SDL.GetError()}");
        }

        var surface = new Surface(handle);
        return surface;
    }

    public static Surface LockTexture(Texture texture, nint rectangle)
    {
        var result = SDL.LockTextureToSurface(texture.Handle, rectangle, out var handle);
        if (!result || handle == nint.Zero)
        {
            throw new Exception($"Couldn't lock texture: {SDL.GetError()}");
        }

        var surface = new Surface(handle);
        return surface;
    }

    public static Surface LockTexture(Texture texture, SDL.Rect rectangle)
    {
        var result = SDL.LockTextureToSurface(texture.Handle, rectangle, out var handle);
        if (!result || handle == nint.Zero)
        {
            throw new Exception($"Couldn't lock texture: {SDL.GetError()}");
        }

        var surface = new Surface(handle);
        return surface;
    }

    public bool BlitSurface(Surface destination, nint sourceRectangle, nint destinationRectangle)
    {
        return SDL.BlitSurface(Handle, sourceRectangle, destination.Handle, destinationRectangle);
    }

    public bool BlitSurface(Surface destination, nint sourceRectangle, SDL.Rect destinationRectangle)
    {
        return SDL.BlitSurface(Handle, sourceRectangle, destination.Handle, destinationRectangle);
    }

    public bool BlitSurface(Surface destination, SDL.Rect sourceRectangle, nint destinationRectangle)
    {
        return SDL.BlitSurface(Handle, sourceRectangle, destination.Handle, destinationRectangle);
    }

    public bool BlitSurface(Surface destination, SDL.Rect sourceRectangle, SDL.Rect destinationRectangle)
    {
        return SDL.BlitSurface(Handle, sourceRectangle, destination.Handle, destinationRectangle);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL.DestroySurface(Handle);

            IsDisposed = true;
        }
    }

    ~Surface()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
