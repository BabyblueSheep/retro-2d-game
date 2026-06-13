using System.Numerics;

namespace Retro2DGame.Core.SDLWrappers;

internal sealed class Window : IDisposable
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vector2 Size { get; private set; }

    public SDL3.SDL.PixelFormat PixelFormat
    {
        get => SDL3.SDL.GetWindowPixelFormat(Handle);
    }

    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    public Window(string title, int width, int height, SDL3.SDL.WindowFlags windowFlags)
    {
        Handle = SDL3.SDL.CreateWindow(title, width, height, windowFlags);

        if (Handle == nint.Zero)
        {
            throw new Exception($"Couldn't create window: {SDL3.SDL.GetError()}");
        }

        Width = width;
        Height = height;
        Size = new Vector2(width, height);
    }

    public bool UpdateWindowSurface()
    {
        return SDL3.SDL.UpdateWindowSurface(Handle);
    }

    public void UpdateWindowSize()
    {
        SDL3.SDL.GetWindowSize(Handle, out var width, out var height);

        Width = width;
        Height = height;
        Size = new Vector2(width, height);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                
            }

            SDL3.SDL.DestroyWindow(Handle);

            IsDisposed = true;
        }
    }

    ~Window()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
