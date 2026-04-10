using SDL3;

namespace Retro2DGame.Core.SDL3.Rendering;

internal sealed class Texture
{
    public nint Handle { get; }

    private Texture(nint handle)
    {
        Handle = handle;
    }

    public static Texture? AcquireSwapchainTexture(CommandBuffer commandBuffer, Window window)
    {
        bool managedToAcquireSwapchainTexture = SDL.SDL_AcquireGPUSwapchainTexture(commandBuffer.Handle, window.Handle, out var handle, out var textureWidth, out var textureHeight);
        if (!managedToAcquireSwapchainTexture)
        {
            SDL.SDL_LogError((int)SDL.SDL_LogCategory.SDL_LOG_CATEGORY_ERROR, $"Couldn't properly acquire swapchain texture: {SDL.SDL_GetError()}");
            return null;
        }
        var swapchainTexture = new Texture(handle);
        return swapchainTexture;
    }
}
