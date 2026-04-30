using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using Retro2DGame.Core.SDL3.Extensions;
using SDL3;
using System.Diagnostics;
using System.Drawing;

namespace Retro2DGame.Core.Game;

internal sealed class GameEngine : IDisposable
{
    public const int DEFAULT_WINDOW_WIDTH = 640;
    public const int DEFAULT_WINDOW_HEIGHT = 480;

    public const int GAME_WIDTH = 256;
    public const int GAME_HEIGHT = 256;

    private readonly Stopwatch _timeTracker;
    private TimeSpan _previousElapsedTime;
    private TimeSpan _accumulatedTime;

    private readonly TimeSpan _tickDuration;
    private readonly int _maxUpdateAmountPerTick;

    private readonly PaletteIndexBitmap _presentingBitmap;
    private Surface _presentingSurface;
    private Renderer _presentingRenderer;

    private Renderer _windowRenderer;
    private Texture _windowTexture;


    public Inputs Inputs { get; }
    public GameStateStack GameStates { get; }
    public AssetKeeper AssetKeeper { get; }

    public Palette Palette { get; private set; }

    public Window Window { get; }

    public bool HasRequestedToDie { get; private set; }

    public bool IsDisposed { get; private set; }

    public GameEngine
    (
        TimeSpan tickDuration, int maxUpdateAmountPerTick
    )
    {
        var windowFlags = SDL.WindowFlags.Resizable;
        Window = new Window("Game", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, windowFlags);
        _windowRenderer = Renderer.Create(Window, "software");
        _windowTexture = Texture.Create(_windowRenderer, Window.PixelFormat, SDL.TextureAccess.Streaming, GAME_WIDTH, GAME_HEIGHT);
        _windowTexture.ScaleMode = SDL.ScaleMode.PixelArt;

        _timeTracker = new Stopwatch();
        _previousElapsedTime = _timeTracker.Elapsed;
        _accumulatedTime = TimeSpan.Zero;

        _tickDuration = tickDuration;
        _maxUpdateAmountPerTick = maxUpdateAmountPerTick;

        _presentingBitmap = PaletteIndexBitmap.CreateEmpty(GAME_WIDTH, GAME_HEIGHT);
        _presentingSurface = Surface.Create(GAME_WIDTH, GAME_HEIGHT, Window.PixelFormat);
        _presentingRenderer = Renderer.CreateSoftware(_presentingSurface);

        Inputs = new Inputs();
        GameStates = new GameStateStack();
        AssetKeeper = new AssetKeeper();

        Palette = new Palette();

        Palette[0, 0] = Color.Transparent;
        Palette[1, 0] = Color.Red;
        Palette[2, 0] = Color.Green;
        Palette[3, 0] = Color.Yellow;

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

    public void Run()
    {
        Inputs.Propagate();
        while (SDL.PollEvent(out var @event))
        {
            if ((SDL.EventType)@event.Type == SDL.EventType.WindowPixelSizeChanged)
            {
                _windowRenderer.Dispose();

                _windowRenderer = Renderer.Create(Window, "software");
                _windowTexture = Texture.Create(_windowRenderer, Window.PixelFormat, SDL.TextureAccess.Streaming, GAME_WIDTH, GAME_HEIGHT);
                _windowTexture.ScaleMode = SDL.ScaleMode.PixelArt;

                _windowRenderer.SetDrawColorFloat(Color.Black.ToFColor());
                _windowRenderer.Clear();
            }

            if ((SDL.EventType)@event.Type == SDL.EventType.KeyDown || (SDL.EventType)@event.Type == SDL.EventType.KeyUp)
            {
                Inputs.UpdateEvent(@event);
            }

            if ((SDL.EventType)@event.Type == SDL.EventType.Quit)
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

        _presentingBitmap.Clear();

        var currentGameStatesCopy = GameStates.Copy();
        foreach (var state in currentGameStatesCopy)
        {
            state.Update(deltaTime);

            for (int i = 0; i < timesToUpdateThisFrame; i++)
            {
                state.FixedUpdate(_tickDuration);
            }

            state.Render(frameProgress, _presentingBitmap);
        }

        PresentBitmap();

        Window.UpdateWindowSurface();
    }

    private void PresentBitmap()
    {
        _presentingRenderer.SetDrawColorFloat(Color.Black.ToFColor());
        _presentingRenderer.Clear();

        _windowRenderer.SetDrawColorFloat(Color.Black.ToFColor());
        _windowRenderer.Clear();
        SDL.SetRenderLogicalPresentation(_windowRenderer.Handle, GAME_WIDTH, GAME_HEIGHT, SDL.RendererLogicalPresentation.Letterbox);

        _presentingRenderer.BlitPaletteIndexBitmap(_presentingBitmap, 0, 0, Palette);
        _presentingRenderer.Present();

        var windowTextureSurface = Surface.LockTexture(_windowTexture, nint.Zero);

        _presentingSurface.BlitSurface(windowTextureSurface, new SDL.Rect() { X = 0, Y = 0, W = GAME_WIDTH, H = GAME_HEIGHT }, nint.Zero);

        _windowTexture.Unlock();

        _windowRenderer.RenderTexture(_windowTexture, new SDL.FRect() { X = 0, Y = 0, W = GAME_WIDTH, H = GAME_HEIGHT }, nint.Zero);

        _windowRenderer.Present();
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
                
            }

            _presentingSurface.Dispose();
            _presentingRenderer.Dispose();

            _windowRenderer.Dispose();
            Window.Dispose();

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
