namespace Retro2DGame.Core.Game;

internal abstract class GameState : IDisposable
{
    public GameEngine GameEngine { get; }

    public bool IsDisposed { get; protected set; }

    public GameState(GameEngine gameEngine)
    {
        GameEngine = gameEngine; 
    }

    public abstract void Update(TimeSpan delta);
    public abstract void FixedUpdate(TimeSpan delta);

    public abstract void Render(double progress);

    protected abstract void Dispose(bool disposing);

    ~GameState()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

internal sealed class GameStateStack
{
    private readonly List<GameState> _gameStates;

    public int Count => _gameStates.Count;

    public GameStateStack()
    {
        _gameStates = [];
    }

    public List<GameState> Copy()
    {
        return _gameStates.GetRange(0, _gameStates.Count);
    }

    public void Push(GameState gameState)
    {
        _gameStates.Add(gameState);
    }

    public GameState Peek()
    {
        var gameState = _gameStates[^1];
        return gameState;
    }

    public GameState Pop()
    {
        var gameState = _gameStates[^1];
        _gameStates.RemoveAt(_gameStates.Count - 1);
        return gameState;
    }
}