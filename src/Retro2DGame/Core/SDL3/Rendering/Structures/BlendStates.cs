using SDL3;

namespace Retro2DGame.Core.SDL3.Rendering.Structures;

internal static class BlendStates
{
    extension(SDL.SDL_SDL_GPUColorTargetBlendState blendState)
    {
        public static SDL.SDL_GPUColorTargetBlendState Opaque => new SDL.SDL_GPUColorTargetBlendState()
        {
            enable_blend = true,
            alpha_blend_op = SDL.SDL_GPUBlendOp.SDL_GPU_BLENDOP_ADD,
            color_blend_op = SDL.SDL_GPUBlendOp.SDL_GPU_BLENDOP_ADD,
            src_color_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE,
            src_alpha_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE,
            dst_color_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ZERO,
            dst_alpha_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ZERO
        };

        public static SDL.SDL_GPUColorTargetBlendState Additive => new SDL.SDL_GPUColorTargetBlendState()
        {
            enable_blend = true,
            alpha_blend_op = SDL.SDL_GPUBlendOp.SDL_GPU_BLENDOP_ADD,
            color_blend_op = SDL.SDL_GPUBlendOp.SDL_GPU_BLENDOP_ADD,
            src_color_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_ALPHA,
            src_alpha_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_ALPHA,
            dst_color_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE,
            dst_alpha_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE
        };

        public static SDL.SDL_GPUColorTargetBlendState AlphaBlend => new SDL.SDL_GPUColorTargetBlendState()
        {
            enable_blend = true,
            alpha_blend_op = SDL.SDL_GPUBlendOp.SDL_GPU_BLENDOP_ADD,
            color_blend_op = SDL.SDL_GPUBlendOp.SDL_GPU_BLENDOP_ADD,
            src_color_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE,
            src_alpha_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE,
            dst_color_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_ALPHA,
            dst_alpha_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_ALPHA
        };

        public static SDL.SDL_GPUColorTargetBlendState NonPremultiplied => new SDL.SDL_GPUColorTargetBlendState()
        {
            enable_blend = true,
            alpha_blend_op = SDL.SDL_GPUBlendOp.SDL_GPU_BLENDOP_ADD,
            color_blend_op = SDL.SDL_GPUBlendOp.SDL_GPU_BLENDOP_ADD,
            src_color_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_ALPHA,
            src_alpha_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_SRC_ALPHA,
            dst_color_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_ALPHA,
            dst_alpha_blendfactor = SDL.SDL_GPUBlendFactor.SDL_GPU_BLENDFACTOR_ONE_MINUS_SRC_ALPHA
        };
    }
}
