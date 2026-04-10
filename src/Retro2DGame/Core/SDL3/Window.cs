using SDL3;

namespace Retro2DGame.Core.SDL3;

internal sealed class Window : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    public Window(string title, int width, int height, SDL.SDL_WindowFlags windowFlags)
    {
        Handle = SDL.SDL_CreateWindow(title, width, height, windowFlags);

        if (Handle == nint.Zero)
        {
            throw new Exception($"Couldn't create window: {SDL.SDL_GetError()}");
        }
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                
            }

            SDL.SDL_DestroyWindow(Handle);

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
