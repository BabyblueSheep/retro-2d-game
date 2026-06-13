using System.Drawing;

namespace Retro2DGame.Core.SDLWrappers;

internal sealed class Renderer : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private Renderer(nint handle)
    {
        Handle = handle;
    }

    public static Renderer Create(Window window, string? name)
    {
        var handle = SDL3.SDL.CreateRenderer(window.Handle, name);

        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create renderer: {SDL3.SDL.GetError()}");
        }

        var renderer = new Renderer(handle);
        return renderer;
    }

    public static Renderer CreateSoftware(Surface surface)
    {
        var handle = SDL3.SDL.CreateSoftwareRenderer(surface.Handle);

        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create renderer: {SDL3.SDL.GetError()}");
        }

        var renderer = new Renderer(handle);
        return renderer;
    }

    public bool Clear()
    {
        return SDL3.SDL.RenderClear(Handle);
    }

    public bool SetDrawColorFloat(float r, float g, float b, float a)
    {
        return SDL3.SDL.SetRenderDrawColorFloat(Handle, r, g, b, a);
    }
    public bool SetDrawColorFloat(SDL3.SDL.FColor color) => SetDrawColorFloat(color.R, color.G, color.B, color.A);

    public bool SetDrawColor(byte r, byte g, byte b, byte a)
    {
        return SDL3.SDL.SetRenderDrawColor(Handle, r, g, b, a);
    }
    public bool SetDrawColor(SDL3.SDL.Color color) => SetDrawColor(color.R, color.G, color.B, color.A);
    public bool SetDrawColor(Color color) => SetDrawColor(color.R, color.G, color.B, color.A);

    public bool Present()
    {
        return SDL3.SDL.RenderPresent(Handle);
    }

    public bool RenderPoint(int x, int y)
    {
        return SDL3.SDL.RenderPoint(Handle, x, y);
    }

    public bool RenderTexture(Texture texture, nint sourceRectangle, nint destinationRectangle)
    {
        return SDL3.SDL.RenderTexture(Handle, texture.Handle, sourceRectangle, destinationRectangle);
    }

    public bool RenderTexture(Texture texture, SDL3.SDL.FRect sourceRectangle, nint destinationRectangle)
    {
        return SDL3.SDL.RenderTexture(Handle, texture.Handle, sourceRectangle, destinationRectangle);
    }

    public bool RenderTexture(Texture texture, nint sourceRectangle, SDL3.SDL.FRect destinationRectangle)
    {
        return SDL3.SDL.RenderTexture(Handle, texture.Handle, sourceRectangle, destinationRectangle);
    }

    public bool RenderTexture(Texture texture, SDL3.SDL.FRect sourceRectangle, SDL3.SDL.FRect destinationRectangle)
    {
        return SDL3.SDL.RenderTexture(Handle, texture.Handle, sourceRectangle, destinationRectangle);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                
            }

            SDL3.SDL.DestroyRenderer(Handle);

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
