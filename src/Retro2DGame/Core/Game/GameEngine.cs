using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using Retro2DGame.Core.SDL3.Extensions;
using SDL3;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace Retro2DGame.Core.Game;

internal sealed class GameEngine : IDisposable
{
    public const int DEFAULT_WINDOW_WIDTH = 256 * 2;
    public const int DEFAULT_WINDOW_HEIGHT = 256 * 2;
    public readonly Vector2 DEFAULT_WINDOW_SIZE = new Vector2(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT);

    public const int GAME_WIDTH = 256;
    public const int GAME_HEIGHT = 256;
    public readonly Vector2 GAME_SIZE = new Vector2(GAME_WIDTH, GAME_HEIGHT);

    private readonly Stopwatch _timeTracker;
    private TimeSpan _previousElapsedTime;
    private TimeSpan _accumulatedTime;

    private readonly TimeSpan _tickDuration;
    private readonly int _maxUpdateAmountPerTick;

    private readonly Surface _presentingSurface;
    private readonly Renderer _presentingRenderer;

    private readonly Renderer _windowRenderer;
    private readonly Texture _windowTexture;


    public Inputs Inputs { get; }
    public GameStateStack GameStates { get; }
    public AssetStorage AssetStorage { get; }

    public Palette Palette { get; }
    public PaletteIndexBitmap Bitmap { get; }

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
        _presentingSurface = Surface.Create(GAME_WIDTH, GAME_HEIGHT, Window.PixelFormat);
        _presentingRenderer = Renderer.CreateSoftware(_presentingSurface);

        Inputs = new Inputs();
        GameStates = new GameStateStack();
        AssetStorage = new AssetStorage();

        Palette = new Palette(); 
        Bitmap = PaletteIndexBitmap.CreateEmpty(GAME_WIDTH, GAME_HEIGHT);


        Palette[0, 0] = Color.Transparent;
        Palette[1, 0] = Color.Red;
        Palette[2, 0] = Color.Green;
        Palette[3, 0] = Color.Yellow;

        Palette[19, 0] = Color.MediumPurple;
        Palette[20, 0] = Color.Purple;
        Palette[21, 0] = Color.Black;

        Palette[31, 0] = Color.White; Palette[31, 1] = Color.Moccasin; Palette[31, 2] = Color.SandyBrown;
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
            switch ((SDL.EventType)@event.Type)
            {
                case SDL.EventType.WindowResized:
                    break;
                case SDL.EventType.WindowPixelSizeChanged:
                    Window.UpdateWindowSize();

                    //_windowRenderer = Renderer.Create(Window, "software");
                    //_windowTexture.Dispose();
                    //_windowTexture = Texture.Create(_windowRenderer, Window.PixelFormat, SDL.TextureAccess.Streaming, GAME_WIDTH, GAME_HEIGHT);
                    //_windowTexture.ScaleMode = SDL.ScaleMode.PixelArt;

                    //_windowRenderer.SetDrawColorFloat(Color.Black.ToFColor());
                    //_windowRenderer.Clear();
                    break;

                case SDL.EventType.KeyDown:
                case SDL.EventType.KeyUp:
                case SDL.EventType.MouseMotion:
                case SDL.EventType.MouseButtonDown:
                case SDL.EventType.MouseButtonUp:
                    Inputs.UpdateEvent(@event, Window.Size, GAME_SIZE);
                    break;
                    
                case SDL.EventType.Quit:
                    RequestToDie();
                    break;
                default:
                    break;

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

        Bitmap.Clear();

        var currentGameStatesCopy = GameStates.Copy();
        foreach (var state in currentGameStatesCopy)
        {
            state.Update(deltaTime);

            for (int i = 0; i < timesToUpdateThisFrame; i++)
            {
                state.FixedUpdate(_tickDuration);
            }

            state.Render(frameProgress);
        }

        PresentBitmap();

        Window.UpdateWindowSurface();
    }

    private void PresentBitmap()
    {
        _presentingRenderer.SetDrawColorFloat(Color.Black.ToFColor());
        _presentingRenderer.Clear();

        SDL.SetRenderLogicalPresentation(_windowRenderer.Handle, Window.Width, Window.Height, SDL.RendererLogicalPresentation.Disabled);
        _windowRenderer.SetDrawColorFloat(Color.Black.ToFColor());
        _windowRenderer.Clear();

        SDL.SetRenderLogicalPresentation(_windowRenderer.Handle, GAME_WIDTH, GAME_HEIGHT, SDL.RendererLogicalPresentation.Letterbox);
        _windowRenderer.SetDrawColorFloat(Color.White.ToFColor());
        _windowRenderer.Clear();

        _presentingRenderer.BlitPaletteIndexBitmap(Bitmap, 0, 0, Palette);
        _presentingRenderer.Present();

        if (Surface.LockTexture(_windowTexture, nint.Zero, out var windowTextureSurface))
        {
            _presentingSurface.BlitSurface(windowTextureSurface!, new SDL.Rect() { X = 0, Y = 0, W = GAME_WIDTH, H = GAME_HEIGHT }, nint.Zero);

            _windowTexture.Unlock();
        }

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
