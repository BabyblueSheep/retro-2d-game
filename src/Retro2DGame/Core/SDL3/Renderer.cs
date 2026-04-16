using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.SDL3;

internal sealed class Renderer : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    public Renderer(Window window, string? name)
    {
        Handle = SDL.CreateRenderer(window.Handle, name);


        if (Handle == nint.Zero)
        {
            throw new Exception($"Couldn't create renderer: {SDL.GetError()}");
        }
    }

    public bool Clear()
    {
        return SDL.RenderClear(Handle);
    }

    public bool SetDrawColorFloat(float r, float g, float b, float a)
    {
        return SDL.SetRenderDrawColorFloat(Handle, r, g, b, a);
    }
    public bool SetDrawColorFloat(SDL.FColor color) => SetDrawColorFloat(color.R, color.G, color.B, color.A);

    public bool Present()
    {
        return SDL.RenderPresent(Handle);
    }

    public bool RenderTexture(Texture texture, SDL.FRect sourceRectangle, SDL.FRect destinationRectangle)
    {
        return SDL.RenderTexture(Handle, texture.Handle, sourceRectangle, destinationRectangle);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                
            }

            SDL.DestroyRenderer(Handle);

            IsDisposed = true;
        }
    }

    ~Renderer()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
