using SDL3;

namespace Retro2DGame.Core.SDL3.Rendering;

internal sealed class GraphicsDevice : IDisposable
{
    public nint Handle { get; }

    public string Backend { get; }

    public bool IsDisposed { get; private set; }

    public GraphicsDevice(SDL.SDL_GPUShaderFormat formatFlags, bool debugMode, string? name)
    {
        Handle = SDL.SDL_CreateGPUDevice(formatFlags, debugMode, name);

        if (Handle == nint.Zero)
        {
            throw new Exception($"Couldn't create GraphicsDevice: {SDL.SDL_GetError()}");
        }

        var backend = SDL.SDL_GetGPUDeviceDriver(Handle) ?? throw new Exception($"Couldn't get GraphicsDevice backend: {SDL.SDL_GetError()}");
        Backend = backend;
    }

    public void ClaimWindow(Window window)
    {
        SDL.SDL_ClaimWindowForGPUDevice(Handle, window.Handle);
    }

    public void UnclaimWindow(Window window)
    {
        SDL.SDL_ReleaseWindowFromGPUDevice(Handle, window.Handle);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                SDL.SDL_DestroyGPUDevice(Handle);
            }

            IsDisposed = true;
        }
    }

    ~GraphicsDevice()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
