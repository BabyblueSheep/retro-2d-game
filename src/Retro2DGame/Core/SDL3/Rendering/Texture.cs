using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.SDL3.Rendering;

internal sealed class Texture
{
    public nint Handle { get; }

    public SDL.SDL_GPUTextureFormat Format { get; }

    private Texture(nint handle, SDL.SDL_GPUTextureFormat format)
    {
        Handle = handle;

        Format = format;
    }

    public static Texture? WaitAndAcquireSwapchainTexture(CommandBuffer commandBuffer, GraphicsDevice device, Window window)
    {
        bool managedToAcquireSwapchainTexture = SDL.SDL_WaitAndAcquireGPUSwapchainTexture(commandBuffer.Handle, window.Handle, out var handle, out var textureWidth, out var textureHeight);
        if (!managedToAcquireSwapchainTexture)
        {
            SDL.SDL_LogError((int)SDL.SDL_LogCategory.SDL_LOG_CATEGORY_ERROR, $"Couldn't properly acquire swapchain texture: {SDL.SDL_GetError()}");
            return null;
        }
        var swapchainTexture = new Texture(handle, SDL.SDL_GetGPUSwapchainTextureFormat(device.Handle, window.Handle));
        return swapchainTexture;
    }
}
