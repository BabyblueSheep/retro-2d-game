using SDL3;
using System.Runtime.InteropServices;

namespace Retro2DGame.Core.SDL3;

internal sealed class Texture : IDisposable
{
    [StructLayout(LayoutKind.Sequential)]
    private struct SDLTexture
    {
        public SDL.PixelFormat Format;

        public int Width;
        public int Height;

        public int Refcount;
    }

    public int Width
    {
        get
        {
            var structure = SDL.PointerToStructure<SDLTexture>(Handle);
            if (structure == null)
                return 0;
            return structure.Value.Width;
        }
    }

    public int Height
    {
        get
        {
            var structure = SDL.PointerToStructure<SDLTexture>(Handle);
            if (structure == null)
                return 0;
            return structure.Value.Height;
        }
    }

    public SDL.ScaleMode ScaleMode
    {
        get
        {
            SDL.GetTextureScaleMode(Handle, out var scaleMode);
            return scaleMode;
        }

        set
        {
            SDL.SetTextureScaleMode(Handle, value);
        }
    }

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
