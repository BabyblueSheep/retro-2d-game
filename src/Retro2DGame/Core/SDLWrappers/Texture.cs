using System.Runtime.InteropServices;

namespace Retro2DGame.Core.SDLWrappers;

internal sealed class Texture : IDisposable
{
    [StructLayout(LayoutKind.Sequential)]
    private struct SDLTexture
    {
        public SDL3.SDL.PixelFormat Format;

        public int Width;
        public int Height;

        public int Refcount;
    }

    public int Width
    {
        get
        {
            var structure = SDL3.SDL.PointerToStructure<SDLTexture>(Handle);
            if (structure == null)
                return 0;
            return structure.Value.Width;
        }
    }

    public int Height
    {
        get
        {
            var structure = SDL3.SDL.PointerToStructure<SDLTexture>(Handle);
            if (structure == null)
                return 0;
            return structure.Value.Height;
        }
    }

    public SDL3.SDL.ScaleMode ScaleMode
    {
        get
        {
            SDL3.SDL.GetTextureScaleMode(Handle, out var scaleMode);
            return scaleMode;
        }

        set
        {
            SDL3.SDL.SetTextureScaleMode(Handle, value);
        }
    }

    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private Texture(nint handle)
    {
        Handle = handle; 
    }

    public static Texture Create(Renderer renderer, SDL3.SDL.PixelFormat format, SDL3.SDL.TextureAccess access, int width, int height)
    {
        var handle = SDL3.SDL.CreateTexture(renderer.Handle, format, access, width, height);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create texture: {SDL3.SDL.GetError()}");
        }
        var texture = new Texture(handle);
        return texture;
    }

    public static Texture CreateFromSurface(Renderer renderer, Surface surface)
    {
        var handle = SDL3.SDL.CreateTextureFromSurface(renderer.Handle, surface.Handle);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create texture: {SDL3.SDL.GetError()}");
        }
        var texture = new Texture(handle);
        return texture;
    }

    public bool Lock(nint rectangle, out nint pixels, out int pitch)
    {
        var result = SDL3.SDL.LockTexture(Handle, rectangle, out var _pixels, out var _pitch);
        pixels = _pixels;
        pitch = _pitch;
        return result;
    }

    public bool Lock(SDL3.SDL.Rect rectangle, out nint pixels, out int pitch)
    {
        var result = SDL3.SDL.LockTexture(Handle, rectangle, out var _pixels, out var _pitch);
        pixels = _pixels;
        pitch = _pitch;
        return result;
    }

    public void Unlock()
    {
        SDL3.SDL.UnlockTexture(Handle);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL3.SDL.DestroyTexture(Handle);

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
