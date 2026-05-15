using SDL3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;

namespace Retro2DGame.Core.SDL3;

internal sealed class Window : IDisposable
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vector2 Size { get; private set; }

    public SDL.PixelFormat PixelFormat
    {
        get => SDL.GetWindowPixelFormat(Handle);
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

        Width = width;
        Height = height;
        Size = new Vector2(width, height);
    }

    public bool UpdateWindowSurface()
    {
        return SDL.UpdateWindowSurface(Handle);
    }

    public void UpdateWindowSize()
    {
        SDL.GetWindowSize(Handle, out var width, out var height);

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
