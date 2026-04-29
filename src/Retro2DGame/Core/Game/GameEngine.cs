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

    private Surface _windowSurface;
    private Renderer _windowRenderer;
    private Texture _windowTexture;


    public Inputs Inputs { get; }
    public GameStateStack GameStates { get; }
    public AssetKeeper AssetKeeper { get; }

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
        _windowTexture = Texture.Create(_windowRenderer, Window.PixelFormat, SDL.TextureAccess.Streaming, Window.Width, Window.Height);

        _timeTracker = new Stopwatch();
        _previousElapsedTime = _timeTracker.Elapsed;
        _accumulatedTime = TimeSpan.Zero;

        _tickDuration = tickDuration;
        _maxUpdateAmountPerTick = maxUpdateAmountPerTick;

        _presentingBitmap = PaletteIndexBitmap.CreateEmpty(GAME_WIDTH, GAME_HEIGHT);

        Inputs = new Inputs();
        GameStates = new GameStateStack();
        AssetKeeper = new AssetKeeper();
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

                _windowSurface = Surface.GetFromWindow(Window);
                _windowRenderer = Renderer.CreateSoftware(_windowSurface);
                _windowTexture = Texture.Create(_windowRenderer, Window.PixelFormat, SDL.TextureAccess.Streaming, Window.Width, Window.Height);

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
        _windowRenderer.SetDrawColorFloat(Color.Black.ToFColor());
        _windowRenderer.Clear();

        SDL.SetRenderLogicalPresentation(_windowRenderer.Handle, GAME_WIDTH, GAME_HEIGHT, SDL.RendererLogicalPresentation.Letterbox);

        /*
        _presentingRenderer.SetDrawColorFloat(Color.Black.ToFColor());
        _presentingRenderer.Clear();

        //SDL.SetRenderLogicalPresentation(_windowRenderer.Handle, GAME_WIDTH, GAME_HEIGHT, SDL.RendererLogicalPresentation.Letterbox);

        /*_presentingRenderer.BlitPaletteIndexBitmap(_presentingBitmap, 0, 0, new Color[,]
        {
            { Color.Transparent },
            { Color.Red },
            { Color.Green },
            { Color.Yellow }
        });

        var t = _presentingRenderer.SetDrawColorFloat(Color.Red.ToFColor());
        SDL.RenderFillRect(_presentingRenderer.Handle, new SDL.FRect() { X = 0, Y = 0, W = 200, H = 200 });

        _presentingRenderer.Present();

        _windowRenderer.SetDrawColorFloat(Color.White.ToFColor());
        SDL.RenderRect(_windowRenderer.Handle, new SDL.FRect() { X = 0, Y = 0, W = 100, H = 100 });

        _presentingSurface.BlitSurface(_windowSurface, nint.Zero, nint.Zero);*/

        _windowTexture.Lock(nint.Zero, out nint pixelsHandle, out int pitch);
        var pixelCount = (pitch / 4) * GAME_HEIGHT;
        var pixels = SDL.PointerToStructureArray<int>(pixelsHandle, pixelCount);
        if (pixels == null)
        {
            throw new Exception("Pixels is null!");
        }

        for (int y = 0; y < GAME_HEIGHT; y++)
        {
            for (int x = 0; x < pitch / 4; x++)
            {
                pixels[x + (y * GAME_HEIGHT)] = (255 << 24) | (255 << 16) | (255 << 8) | (255 << 0);
            }
        }

        _windowTexture.Unlock();

        SDL.RenderTexture(_windowRenderer.Handle, _windowTexture.Handle, new SDL.FRect() { X = 0, Y = 0, W = GAME_WIDTH, H = GAME_WIDTH }, nint.Zero);

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
