using Game.Core.SDL3.Rendering.Structures;
using SDL3;

namespace Retro2DGame.Core.SDL3.Rendering;

internal sealed class GraphicsPipeline
{
    public nint Handle { get; }

    private GraphicsPipeline(nint handle)
    {
        Handle = handle;
    }

    public static GraphicsPipeline Create<T>(
        GraphicsDevice device,
        SDL.SDL_GPUTextureFormat textureFormat,

        Shader vertexShader, Shader fragmentShader,
        SDL.SDL_GPURasterizerState rasterizerState,
        SDL.SDL_GPUColorTargetBlendState blendState
    ) where T : IVertexType
    {
        var colortargetDescriptionsPointer = SDL.SDL_StructureArrayToPointer
        ([
            new SDL.SDL_GPUColorTargetDescription()
            {
                Format = textureFormat
            }
        ]);

        /*var vertexAttributes = new SDL.SDL_GPUVertexAttribute[T.Offsets.Length];
        for (uint i = 0; i < vertexAttributes.Length; i++)
        {
            vertexAttributes[i] = new SDL.SDL_GPUVertexAttribute()
            {
                BufferSlot = 0,

                Location = i,
                Format = T.Formats[i],
                Offset = T.Offsets[i],
            };
        }*/

        var vertexAttributesPointer = SDL.SDL_StructureArrayToPointer
        ([
            new SDL.SDL_GPUVertexAttribute()
            {
                BufferSlot = 0,

                Location = 0,
                Format = SDL.SDL_GPUVertexElementFormat.Float3,
                Offset = 0,
            },
            new SDL.SDL_GPUVertexAttribute()
            {
                BufferSlot = 0,

                Location = 1,
                Format = SDL.SDL_GPUVertexElementFormat.Ubyte4Norm,
                Offset = 16,
            }
        ]);

        var vertexBufferDescriptionsPointer = SDL.SDL_StructureArrayToPointer
        ([
            new SDL.SDL_GPUVertexBufferDescription()
            {
                Slot = 0,
                InputRate = SDL.SDL_GPUVertexInputRate.Vertex,
                Pitch = 32,

                InstanceStepRate = 0,
            }
        ]);

        var graphicsPipelineCreateInfo = new SDL.SDL_GPUGraphicsPipelineCreateInfo()
        {
            vertex_shader = vertexShader.Handle,
            fragment_shader = fragmentShader.Handle,

            primitive_type = SDL.SDL_GPUPrimitiveType.SDL_GPU_PRIMITIVETYPE_TRIANGLELIST,

            vertex_input_state = new SDL.SDL_GPUVertexInputState()
            {
                vertex_buffer_descriptions = vertexBufferDescriptionsPointer,
                num_vertex_buffers = 1,

                vertex_attributes = vertexAttributesPointer,
                num_vertex_attributes = 2,
            },

            target_info = new SDL.SDL_GPUGraphicsPipelineTargetInfo()
            {
                color_target_descriptions = colortargetDescriptionsPointer,
                num_color_targets = 1,
            },

            /*VertexShader = vertexShader.Handle,
            FragmentShader = fragmentShader.Handle,

            VertexInputState = new SDL.SDL_GPUVertexInputState()
            {
                VertexBufferDescriptions = vertexBufferDescriptionsPointer,
                NumVertexBuffers = 1,

                VertexAttributes = vertexAttributesPointer,
                NumVertexAttributes = (uint)vertexAttributes.Length,
            },
            PrimitiveType = SDL.SDL_GPUPrimitiveType.TriangleList,

            RasterizerState = rasterizerState,

            TargetInfo = new SDL.SDL_GPUGraphicsPipelineTargetInfo()
            {
                ColorTargetDescriptions = colortargetDescriptionsPointer,
                NumColorTargets = 1,
            }*/
        };

        var handle = SDL.SDL_CreateGPUGraphicsPipeline(device.Handle, graphicsPipelineCreateInfo);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create GraphicsPipeline: {SDL.SDL_GetError()}");
        }
        var graphicsPipeline = new GraphicsPipeline(handle);

        //Marshal.FreeHGlobal(vertexAttributesPointer);
        //Marshal.FreeHGlobal(vertexBufferDescriptionsPointer);
        //Marshal.FreeHGlobal(colortargetDescriptionsPointer);

        return graphicsPipeline;
    }
}
