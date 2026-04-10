using Game.Content.GameStates;
using Game.Core.Game;
using SDL3;

namespace Retro2DGame;

internal sealed class Program
{
    public const int DEFAULT_WINDOW_WIDTH = 640;
    public const int DEFAULT_WINDOW_HEIGHT = 480;

    public const int GAME_WIDTH = 300;
    public const int GAME_HEIGHT = 200;

    static void Main(string[] args)
    {
        SDL.SDL_SetAppMetadata("Game", "v1", "game");

        SDL.SDL_SetLogPriorities(SDL.SDL_LogPriority.SDL_LOG_PRIORITY_INVALID);

        if (!SDL.SDL_Init(SDL.SDL_InitFlags.SDL_INIT_VIDEO))
        {
            SDL.SDL_LogError((int)SDL.SDL_LogCategory.SDL_LOG_CATEGORY_APPLICATION, $"Couldn't initialize SDL: {SDL.SDL_GetError()}");
            return;
        }

        var windowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
        var windowHandle = SDL.SDL_CreateWindow("Game", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, windowFlags);
        if (windowHandle == nint.Zero)
        {
            SDL.SDL_LogError((int)SDL.SDL_LogCategory.SDL_LOG_CATEGORY_APPLICATION, $"Couldn't initialize SDL: {SDL.SDL_GetError()}");
            return;
        }

        const int TICKS_PER_SECOND = 20;
        const int MAX_UPDATES_PER_TICK = 6;
        var gameEngine = new GameEngine(TimeSpan.FromSeconds(1 / TICKS_PER_SECOND), MAX_UPDATES_PER_TICK);

        gameEngine.GameStates.Push(new MainMenuState(gameEngine));

        gameEngine.Start();
        while (!gameEngine.HasRequestedToDie)
        {
            gameEngine.Run(window, renderer);
        }
        gameEngine.Stop();

        SDL.DestroyRenderer(renderer);
        SDL.DestroyWindow(window);
    }
}
