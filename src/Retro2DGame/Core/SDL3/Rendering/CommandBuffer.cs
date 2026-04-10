using SDL3;

namespace Retro2DGame.Core.SDL3.Rendering;

internal sealed class CommandBuffer
{
    public nint Handle { get; }

    private CommandBuffer(nint handle)
    {
        Handle = handle;
    }

    public static CommandBuffer? AcquireFromGraphicsDevice(GraphicsDevice device)
    {
        var handle = SDL.SDL_AcquireGPUCommandBuffer(device.Handle);
        if (handle == nint.Zero)
        {
            SDL.SDL_LogError((int)SDL.SDL_LogCategory.SDL_LOG_CATEGORY_ERROR, $"Couldn't properly acquire GPU device: {SDL.SDL_GetError()}");
            return null;
        }
        var commandBuffer = new CommandBuffer(handle);
        return commandBuffer;
    }

    public void Submit()
    {


    }
}
