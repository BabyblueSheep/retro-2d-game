using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using Retro2DGame.Core.SDL3.Extensions;
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

    private readonly PaletteIndexBitmap _presentingBitmap;
    private readonly Surface _presentingSurface;
    private readonly Renderer _presentingRenderer;

    public Inputs Inputs { get; }
    public GameStateStack GameStates { get; }
    public AssetKeeper AssetKeeper { get; }

    public bool HasRequestedToDie { get; private set; }

    public bool IsDisposed { get; private set; }

    public GameEngine
    (
        TimeSpan tickDuration, int maxUpdateAmountPerTick
    )
    {
        _timeTracker = new Stopwatch();
        _previousElapsedTime = _timeTracker.Elapsed;
        _accumulatedTime = TimeSpan.Zero;

        _tickDuration = tickDuration;
        _maxUpdateAmountPerTick = maxUpdateAmountPerTick;

        _presentingBitmap = PaletteIndexBitmap.CreateEmpty(Program.GAME_WIDTH, Program.GAME_HEIGHT);
        _presentingSurface = Surface.Create((int)_presentingBitmap.Width, (int)_presentingBitmap.Height, SDL.PixelFormat.RGBA8888);
        _presentingRenderer = Renderer.CreateSoftware(_presentingSurface);

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

    public void Run(Window window, Renderer renderer)
    {
        Inputs.Propagate();
        while (SDL.PollEvent(out var @event))
        {
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

        PresentBitmap(window, renderer);
    }

    private void PresentBitmap(Window window, Renderer windowRenderer)
    {
        windowRenderer.SetDrawColorFloat(Color.Black.ToFColor());
        windowRenderer.Clear();

        _presentingRenderer.SetDrawColorFloat(Color.Black.ToFColor());
        _presentingRenderer.Clear();

        windowRenderer.BlitPaletteIndexBitmap(_presentingBitmap, 0, 0, new Color[,]
        {
            { Color.Transparent },
            { Color.Red },
            { Color.Green },
            { Color.Yellow }
        });
        windowRenderer.Present();


        /*var presentingTexture = Texture.CreateFromSurface(windowRenderer, _presentingSurface);
        presentingTexture.ScaleMode = SDL.ScaleMode.PixelArt;

        windowRenderer.RenderTexture(
            presentingTexture,
            new SDL.FRect()
            {
                X = 0,
                Y = 0,
                W = presentingTexture.Width,
                H = presentingTexture.Height,
            },
            new SDL.FRect()
            {
                X = 0,
                Y = 0,
                W = window.Width,
                H = window.Height,
            }
        );

        windowRenderer.Present();

        presentingTexture.Dispose();*/
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

            _presentingRenderer.Dispose();
            _presentingSurface.Dispose();

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
