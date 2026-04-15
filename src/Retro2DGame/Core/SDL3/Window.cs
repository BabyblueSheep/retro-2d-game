using SDL3;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Retro2DGame.Core.SDL3;

internal sealed class Window : IDisposable
{
    public int Width
    {
        get
        {
            SDL.GetWindowSize(Handle, out var width, out var height);
            return width;
        }
    }

    public int Height
    {
        get
        {
            SDL.GetWindowSize(Handle, out var width, out var height);
            return height;
        }
    }

    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    public Window(string title, int width, int height, SDL.WindowFlags windowFlags)
    {
        Handle = SDL.CreateWindow(title, width, height, windowFlags);

        if (Handle == nint.Zero)
        {
            throw new Exception($"Couldn't create window: {SDL.GetError()}");
        }
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                
            }

            SDL.DestroyWindow(Handle);

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
