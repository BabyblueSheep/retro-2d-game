using Retro2DGame.Core.SDL3.Rendering;
using SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Game.Core.SDL3.Rendering;

internal sealed class RenderPass
{
    public nint Handle { get; }

    private RenderPass(nint handle)
    {
        Handle = handle;
    }

    public static RenderPass Begin(CommandBuffer commandBuffer, Texture textureTarget, SDL.FColor clearColor)
    {
        var colorTargetInfo = new SDL.GPUColorTargetInfo
        {
            Texture = textureTarget.Handle,
            ClearColor = clearColor,
            LoadOp = SDL.GPULoadOp.Clear,
            StoreOp = SDL.GPUStoreOp.Store,
        };

        var colorTargetInfoPointer = SDL.StructureToPointer<SDL.GPUColorTargetInfo>(colorTargetInfo);

        var handle = SDL.BeginGPURenderPass(commandBuffer.Handle, colorTargetInfoPointer, 1, nint.Zero);
        var renderPass = new RenderPass(handle);

        Marshal.FreeHGlobal(colorTargetInfoPointer);

        return renderPass;
    }
}
