using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.SDL3;

internal sealed class Texture : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private Texture(nint handle)
    {
        Handle = handle; 
    }

    public static Texture CreateFromSurface(Renderer renderer, Surface surface)
    {
        var handle = SDL.CreateTextureFromSurface(renderer.Handle, surface.Handle);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create texture: {SDL.GetError()}");
        }

        var texture = new Texture(handle);
        return texture;
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL.DestroyTexture(Handle);

            IsDisposed = true;
        }
    }

    ~Texture()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
