using SDL3;

namespace Game.Core.SDL3.Rendering.Structures;

internal static class BlendStates
{
    extension(SDL.GPUColorTargetBlendState blendState)
    {
        public static SDL.GPUColorTargetBlendState Opaque => new SDL.GPUColorTargetBlendState()
        {
            EnableBlend = true,
            AlphaBlendOp = SDL.GPUBlendOp.Add,
            ColorBlendOp = SDL.GPUBlendOp.Add,
            SrcColorBlendFactor = SDL.GPUBlendFactor.One,
            SrcAlphaBlendFactor = SDL.GPUBlendFactor.One,
            DstColorBlendFactor = SDL.GPUBlendFactor.Zero,
            DstAlphaBlendFactor = SDL.GPUBlendFactor.Zero
        };

        public static SDL.GPUColorTargetBlendState Additive => new SDL.GPUColorTargetBlendState()
        {
            EnableBlend = true,
            AlphaBlendOp = SDL.GPUBlendOp.Add,
            ColorBlendOp = SDL.GPUBlendOp.Add,
            SrcColorBlendFactor = SDL.GPUBlendFactor.SrcAlpha,
            SrcAlphaBlendFactor = SDL.GPUBlendFactor.SrcAlpha,
            DstColorBlendFactor = SDL.GPUBlendFactor.One,
            DstAlphaBlendFactor = SDL.GPUBlendFactor.One
        };

        public static SDL.GPUColorTargetBlendState AlphaBlend => new SDL.GPUColorTargetBlendState()
        {
            EnableBlend = true,
            AlphaBlendOp = SDL.GPUBlendOp.Add,
            ColorBlendOp = SDL.GPUBlendOp.Add,
            SrcColorBlendFactor = SDL.GPUBlendFactor.One,
            SrcAlphaBlendFactor = SDL.GPUBlendFactor.One,
            DstColorBlendFactor = SDL.GPUBlendFactor.OneMinusSrcAlpha,
            DstAlphaBlendFactor = SDL.GPUBlendFactor.OneMinusSrcAlpha
        };

        public static SDL.GPUColorTargetBlendState NonPremultiplied => new SDL.GPUColorTargetBlendState()
        {
            EnableBlend = true,
            AlphaBlendOp = SDL.GPUBlendOp.Add,
            ColorBlendOp = SDL.GPUBlendOp.Add,
            SrcColorBlendFactor = SDL.GPUBlendFactor.SrcAlpha,
            SrcAlphaBlendFactor = SDL.GPUBlendFactor.SrcAlpha,
            DstColorBlendFactor = SDL.GPUBlendFactor.OneMinusSrcAlpha,
            DstAlphaBlendFactor = SDL.GPUBlendFactor.OneMinusSrcAlpha
        };
    }
}
