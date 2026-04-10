using SDL3;

namespace Game.Core.SDL3.Rendering.Structures;

internal static class RasterizerStates
{
    extension(SDL.GPURasterizerState rasterizerState)
    {
        public static SDL.GPURasterizerState CW_CullFront => new SDL.GPURasterizerState()
        {
            CullMode = SDL.GPUCullMode.Front,
            FrontFace = SDL.GPUFrontFace.Clockwise,
            FillMode = SDL.GPUFillMode.Fill
        };

        public static SDL.GPURasterizerState CW_CullBack => new SDL.GPURasterizerState()
        {
            CullMode = SDL.GPUCullMode.Back,
            FrontFace = SDL.GPUFrontFace.Clockwise,
            FillMode = SDL.GPUFillMode.Fill
        };

        public static SDL.GPURasterizerState CW_CullNone => new SDL.GPURasterizerState()
        {
            CullMode = SDL.GPUCullMode.None,
            FrontFace = SDL.GPUFrontFace.Clockwise,
            FillMode = SDL.GPUFillMode.Fill
        };

        public static SDL.GPURasterizerState CW_Wireframe => new SDL.GPURasterizerState()
        {
            CullMode = SDL.GPUCullMode.None,
            FrontFace = SDL.GPUFrontFace.Clockwise,
            FillMode = SDL.GPUFillMode.Line
        };

        public static SDL.GPURasterizerState CCW_CullFront => new SDL.GPURasterizerState()
        {
            CullMode = SDL.GPUCullMode.Front,
            FrontFace = SDL.GPUFrontFace.CounterClockwise,
            FillMode = SDL.GPUFillMode.Fill
        };

        public static SDL.GPURasterizerState CCW_CullBack => new SDL.GPURasterizerState()
        {
            CullMode = SDL.GPUCullMode.Back,
            FrontFace = SDL.GPUFrontFace.CounterClockwise,
            FillMode = SDL.GPUFillMode.Fill
        };

        public static SDL.GPURasterizerState CCW_CullNone => new SDL.GPURasterizerState()
        {
            CullMode = SDL.GPUCullMode.None,
            FrontFace = SDL.GPUFrontFace.CounterClockwise,
            FillMode = SDL.GPUFillMode.Fill
        };

        public static SDL.GPURasterizerState CCW_Wireframe => new SDL.GPURasterizerState()
        {
            CullMode = SDL.GPUCullMode.None,
            FrontFace = SDL.GPUFrontFace.CounterClockwise,
            FillMode = SDL.GPUFillMode.Line
        };
    }
}
