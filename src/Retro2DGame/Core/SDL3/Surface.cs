using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.SDL3;

internal sealed class Surface : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private Surface(nint handle)
    {
        Handle = handle;
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
