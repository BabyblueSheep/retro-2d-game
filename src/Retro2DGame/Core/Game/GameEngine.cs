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

    public Inputs Inputs { get; }
    public GameStateStack GameStates { get; }

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

        Inputs = new Inputs();
        GameStates = new GameStateStack();
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

        renderer.SetDrawColorFloat(Color.Black.ToFColor());
        renderer.Clear();

        var currentGameStatesCopy = GameStates.Copy();
        foreach (var state in currentGameStatesCopy)
        {
            state.Update(deltaTime);

            for (int i = 0; i < timesToUpdateThisFrame; i++)
            {
                state.FixedUpdate(_tickDuration);
            }

            state.Render(frameProgress, window, renderer);
        }

        renderer.Present();
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
