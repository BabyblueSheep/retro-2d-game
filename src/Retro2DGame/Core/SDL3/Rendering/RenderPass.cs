using SDL3;

namespace Retro2DGame.Core.SDL3.Rendering;

internal sealed class RenderPass
{
    public nint Handle { get; }

    private RenderPass(nint handle)
    {
        Handle = handle;
    }

    public static RenderPass Begin(CommandBuffer commandBuffer, Texture textureTarget, SDL.SDL_FColor clearColor)
    {
        var colorTargetInfo = new SDL.SDL_GPUColorTargetInfo
        {
            texture = textureTarget.Handle,
            clear_color = clearColor,
            load_op = SDL.SDL_GPULoadOp.SDL_GPU_LOADOP_CLEAR,
            store_op = SDL.SDL_GPUStoreOp.SDL_GPU_STOREOP_STORE,
        };

        var colorTargetInfoPointer = SDL.SDL_StructureToPointer<SDL.SDL_GPUColorTargetInfo>(colorTargetInfo);

        var handle = SDL.SDL_BeginGPURenderPass(commandBuffer.Handle, colorTargetInfoPointer, 1, nint.Zero);
        var renderPass = new RenderPass(handle);

        Marshal.FreeHGlobal(colorTargetInfoPointer);

        return renderPass;
    }

    public void End()
    {
        SDL.SDL_EndGPURenderPass(Handle);
    }
}
