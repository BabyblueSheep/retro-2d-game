using SDL3;

namespace Retro2DGame.Core.SDL3;

internal sealed class Surface : IDisposable
{
    public int Width => Structure.Width;

    public int Height => Structure.Height;

    public nint Handle { get; }
    public SDL.Surface Structure { get; }

    public bool IsDisposed { get; private set; }

    private Surface(nint handle)
    {
        Handle = handle;
        var structureNullable = SDL.PointerToStructure<SDL.Surface>(Handle);
        if (!structureNullable.HasValue)
        {
            throw new Exception("Surface pointer was invalid");
        }
        Structure = structureNullable.Value;
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

    public static bool LockTexture(Texture texture, nint rectangle, out Surface? surface)
    {
        var result = SDL.LockTextureToSurface(texture.Handle, rectangle, out var handle);
        if (!result || handle == nint.Zero)
        {
            surface = null;
            return result;
        }

        surface = new Surface(handle);
        return result;
    }

    public static bool LockTexture(Texture texture, SDL.Rect rectangle, out Surface? surface)
    {
        var result = SDL.LockTextureToSurface(texture.Handle, rectangle, out var handle);
        if (!result || handle == nint.Zero)
        {
            surface = null;
            return result;
        }

        surface = new Surface(handle);
        return result;
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

    public bool LockSurface()
    {
        return SDL.LockSurface(Handle);
    }
    public void UnlockSurface()
    {
        SDL.UnlockSurface(Handle);
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
