namespace Retro2DGame.Core.SDLWrappers;

internal sealed class Surface : IDisposable
{
    public int Width => Structure.Width;

    public int Height => Structure.Height;

    public nint Handle { get; }
    public SDL3.SDL.Surface Structure { get; }

    public uint[] PaletteColors { get; }

    public bool IsDisposed { get; private set; }

    private Surface(nint handle)
    {
        Handle = handle;
        var structureNullable = SDL3.SDL.PointerToStructure<SDL3.SDL.Surface>(Handle);
        if (!structureNullable.HasValue)
        {
            throw new Exception("Surface pointer was invalid");
        }
        Structure = structureNullable.Value;

        PaletteColors = new uint[256];
    }

    public static Surface Create(int width, int height, SDL3.SDL.PixelFormat pixelFormat)
    {
        var handle = SDL3.SDL.CreateSurface(width, height, pixelFormat);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create surface: {SDL3.SDL.GetError()}");
        }

        var surface = new Surface(handle);
        return surface;
    }

    public static Surface GetFromWindow(Window window)
    {
        var handle = SDL3.SDL.GetWindowSurface(window.Handle);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create surface: {SDL3.SDL.GetError()}");
        }

        var surface = new Surface(handle);
        return surface;
    }

    public static Surface LoadPNG(string filepath)
    {
        var handle = SDL3.SDL.LoadPNG(filepath);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't load PNG: {SDL3.SDL.GetError()}");
        }

        var surface = new Surface(handle);
        return surface;
    }

    public static bool LockTexture(Texture texture, nint rectangle, out Surface? surface)
    {
        var result = SDL3.SDL.LockTextureToSurface(texture.Handle, rectangle, out var handle);
        if (!result || handle == nint.Zero)
        {
            surface = null;
            return result;
        }

        surface = new Surface(handle);
        return result;
    }

    public static bool LockTexture(Texture texture, SDL3.SDL.Rect rectangle, out Surface? surface)
    {
        var result = SDL3.SDL.LockTextureToSurface(texture.Handle, rectangle, out var handle);
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
        return SDL3.SDL.BlitSurface(Handle, sourceRectangle, destination.Handle, destinationRectangle);
    }

    public bool BlitSurface(Surface destination, nint sourceRectangle, SDL3.SDL.Rect destinationRectangle)
    {
        return SDL3.SDL.BlitSurface(Handle, sourceRectangle, destination.Handle, destinationRectangle);
    }

    public bool BlitSurface(Surface destination, SDL3.SDL.Rect sourceRectangle, nint destinationRectangle)
    {
        return SDL3.SDL.BlitSurface(Handle, sourceRectangle, destination.Handle, destinationRectangle);
    }

    public bool BlitSurface(Surface destination, SDL3.SDL.Rect sourceRectangle, SDL3.SDL.Rect destinationRectangle)
    {
        return SDL3.SDL.BlitSurface(Handle, sourceRectangle, destination.Handle, destinationRectangle);
    }

    public bool LockSurface()
    {
        return SDL3.SDL.LockSurface(Handle);
    }
    public void UnlockSurface()
    {
        SDL3.SDL.UnlockSurface(Handle);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL3.SDL.DestroySurface(Handle);

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
