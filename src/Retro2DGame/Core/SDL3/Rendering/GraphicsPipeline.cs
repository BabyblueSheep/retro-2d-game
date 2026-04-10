using Game.Core.SDL3.Rendering.Structures;
using SDL3;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Game.Core.SDL3.Rendering;

internal sealed class GraphicsPipeline
{
    public nint Handle { get; }

    private GraphicsPipeline(nint handle)
    {
        Handle = handle;
    }

    public static GraphicsPipeline Create<T>(
        GraphicsDevice device,
        SDL.GPUTextureFormat textureFormat,

        Shader vertexShader, Shader fragmentShader,
        SDL.GPURasterizerState rasterizerState,
        SDL.GPUColorTargetBlendState blendState
    ) where T : IVertexType
    {
        var colortargetDescriptionsPointer = SDL.StructureArrayToPointer
        ([
            new SDL.GPUColorTargetDescription()
            {
                Format = textureFormat
            }
        ]);

        /*var vertexAttributes = new SDL.GPUVertexAttribute[T.Offsets.Length];
        for (uint i = 0; i < vertexAttributes.Length; i++)
        {
            vertexAttributes[i] = new SDL.GPUVertexAttribute()
            {
                BufferSlot = 0,

                Location = i,
                Format = T.Formats[i],
                Offset = T.Offsets[i],
            };
        }*/

        var vertexAttributesPointer = SDL.StructureArrayToPointer
        ([
            new SDL.GPUVertexAttribute()
            {
                BufferSlot = 0,

                Location = 0,
                Format = SDL.GPUVertexElementFormat.Float3,
                Offset = 0,
            },
            new SDL.GPUVertexAttribute()
            {
                BufferSlot = 0,

                Location = 1,
                Format = SDL.GPUVertexElementFormat.Ubyte4Norm,
                Offset = 16,
            }
        ]);

        var vertexBufferDescriptionsPointer = SDL.StructureArrayToPointer
        ([
            new SDL.GPUVertexBufferDescription()
            {
                Slot = 0,
                InputRate = SDL.GPUVertexInputRate.Vertex,
                Pitch = 32,

                InstanceStepRate = 0,
            }
        ]);

        var graphicsPipelineCreateInfo = new SDL.GPUGraphicsPipelineCreateInfo()
        {
            VertexShader = vertexShader.Handle,
            FragmentShader = fragmentShader.Handle,

            PrimitiveType = SDL.GPUPrimitiveType.TriangleList,

            VertexInputState = new SDL.GPUVertexInputState()
            {
                VertexBufferDescriptions = vertexBufferDescriptionsPointer,
                NumVertexBuffers = 1,

                VertexAttributes = vertexAttributesPointer,
                NumVertexAttributes = 2,
            },

            TargetInfo = new SDL.GPUGraphicsPipelineTargetInfo()
            {
                ColorTargetDescriptions = colortargetDescriptionsPointer,
                NumColorTargets = 1,
            },

            /*VertexShader = vertexShader.Handle,
            FragmentShader = fragmentShader.Handle,

            VertexInputState = new SDL.GPUVertexInputState()
            {
                VertexBufferDescriptions = vertexBufferDescriptionsPointer,
                NumVertexBuffers = 1,

                VertexAttributes = vertexAttributesPointer,
                NumVertexAttributes = (uint)vertexAttributes.Length,
            },
            PrimitiveType = SDL.GPUPrimitiveType.TriangleList,

            RasterizerState = rasterizerState,

            TargetInfo = new SDL.GPUGraphicsPipelineTargetInfo()
            {
                ColorTargetDescriptions = colortargetDescriptionsPointer,
                NumColorTargets = 1,
            }*/
        };

        var handle = SDL.CreateGPUGraphicsPipeline(device.Handle, graphicsPipelineCreateInfo);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create GraphicsPipeline: {SDL.GetError()}");
        }
        var graphicsPipeline = new GraphicsPipeline(handle);

        //Marshal.FreeHGlobal(vertexAttributesPointer);
        //Marshal.FreeHGlobal(vertexBufferDescriptionsPointer);
        //Marshal.FreeHGlobal(colortargetDescriptionsPointer);

        return graphicsPipeline;
    }
}
