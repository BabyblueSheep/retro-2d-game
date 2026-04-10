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
            throw new Exception($"Couldn't properly acquire GPU device: {SDL.SDL_GetError()}");
        }
        var commandBuffer = new CommandBuffer(handle);
        return commandBuffer;
    }

    public void Submit()
    {
        SDL.SDL_SubmitGPUCommandBuffer(Handle);
    }
}
