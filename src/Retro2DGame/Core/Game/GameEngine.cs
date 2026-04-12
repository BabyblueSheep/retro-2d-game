using Retro2DGame.Core.SDL3;
using Retro2DGame.Core.SDL3.Rendering;
using SDL3;
using System.Diagnostics;
using System.Drawing;

namespace Retro2DGame.Core.Game;

internal sealed class GameEngine : IDisposable
{
    private readonly Stopwatch _timeTracker;
    private TimeSpan _previousElapsedTime;
    private TimeSpan _accumulatedTime;

    private readonly TimeSpan _tickDuration;
    private readonly int _maxUpdateAmountPerTick;

    private GraphicsPipeline _pipeline;

    public GraphicsDevice GraphicsDevice { get; }

    public Inputs Inputs { get; }
    public GameStateStack GameStates { get; }

    public bool HasRequestedToDie { get; private set; }

    public bool IsDisposed { get; private set; }

    public GameEngine
    (
        Window window,
        TimeSpan tickDuration, int maxUpdateAmountPerTick
    )
    {
        _timeTracker = new Stopwatch();
        _previousElapsedTime = _timeTracker.Elapsed;
        _accumulatedTime = TimeSpan.Zero;

        _tickDuration = tickDuration;
        _maxUpdateAmountPerTick = maxUpdateAmountPerTick;

        GraphicsDevice = new GraphicsDevice(SDL.SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_SPIRV | SDL.SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_DXIL | SDL.SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_DXBC | SDL.SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_MSL, true, "direct3d12");
        GraphicsDevice.ClaimWindow(window);

        Inputs = new Inputs();
        GameStates = new GameStateStack();

        var vertexShader = Shader.Load(GraphicsDevice, "PositionColor.vert");
        var fragmentShader = Shader.Load(GraphicsDevice, "SolidColor.frag");

        SDL.SDL_LogInfo(SDL.SDL_LogCategory.SDL_LOG_CATEGORY_GPU, $"{SDL.SDL_GetGPUSwapchainTextureFormat(GraphicsDevice.Handle, window.Handle)}");

        /*_pipeline = GraphicsPipeline.Create<PositionColorVertex>
        (
            GraphicsDevice,
            SDL.SDL_GetGPUSwapchainTextureFormat(GraphicsDevice.Handle, window.Handle),

            vertexShader, fragmentShader,
            SDL.SDL_GPURasterizerState.CCW_CullNone,
            SDL.SDL_GPUColorTargetBlendState.Opaque
        );*/

        //vertexShader.Release(GraphicsDevice);
        //fragmentShader.Release(GraphicsDevice);
    }

    public void Start()
    {
        _timeTracker.Start();

        Inputs.Reset();
    }

    public void Stop()
    {
        while (GameStates.Count > 0)
        {
            var gameState = GameStates.Pop();
            gameState.Dispose();
        }

        _timeTracker.Stop();
        _timeTracker.Reset();
        _previousElapsedTime = _timeTracker.Elapsed;
        _accumulatedTime = TimeSpan.Zero;
    }

    public void Run(Window window)
    {
        Inputs.Propagate();
        while (SDL.SDL_PollEvent(out var @event))
        {
            if ((SDL.SDL_EventType)@event.Type == SDL.SDL_EventType.KeyDown || (SDL.SDL_EventType)@event.Type == SDL.SDL_EventType.KeyUp)
            {
                Inputs.UpdateEvent(@event);
            }

            if ((SDL.SDL_EventType)@event.Type == SDL.SDL_EventType.Quit)
            {
                RequestToDie();
            }
        }

        var nextElapsedTime = _timeTracker.Elapsed;
        var deltaTime = nextElapsedTime - _previousElapsedTime;
        _previousElapsedTime = nextElapsedTime;
        _accumulatedTime += deltaTime;

        var timesToUpdateThisFrame = 0;
        while (_accumulatedTime > _tickDuration)
        {
            timesToUpdateThisFrame++;
            _accumulatedTime -= _tickDuration;
        }
        timesToUpdateThisFrame = int.Min(timesToUpdateThisFrame, _maxUpdateAmountPerTick);

        var frameProgress = _accumulatedTime / _tickDuration;

        var commandBuffer = CommandBuffer.AcquireFromGraphicsDevice(GraphicsDevice);
        Debug.Assert(commandBuffer != null);
        var swapchainTexture = Texture.WaitAndAcquireSwapchainTexture(commandBuffer, GraphicsDevice, window);

        RenderPass? renderPass = null;

        if (swapchainTexture != null)
        {
            renderPass = RenderPass.Begin(commandBuffer, swapchainTexture, Color.Red.ToFColor());
        }

        var currentGameStatesCopy = GameStates.Copy();
        foreach (var state in currentGameStatesCopy)
        {
            state.Update(deltaTime);

            for (int i = 0; i < timesToUpdateThisFrame; i++)
            {
                state.FixedUpdate(_tickDuration);
            }

            if (swapchainTexture != null)
            {
                state.Render(frameProgress);
            }
        }



        renderPass?.End();

        commandBuffer.Submit();
    }



    public void RequestToDie()
    {
        HasRequestedToDie = true;
    }



    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                GraphicsDevice.Dispose();
            }

            IsDisposed = true;
        }
    }

    ~GameEngine()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
