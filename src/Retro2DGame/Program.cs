using Retro2DGame.Core.SDL3;
using Retro2DGame.Core.Game;
using Retro2DGame.Content.GameStates;
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
        SDL.SDL_SetAppMetadataProperty(SDL.SDL_PROP_APP_METADATA_NAME_STRING, "Retro 2D Game Game");
        SDL.SDL_SetAppMetadataProperty(SDL.SDL_PROP_APP_METADATA_VERSION_STRING, "v1.0");
        SDL.SDL_SetAppMetadataProperty(SDL.SDL_PROP_APP_METADATA_IDENTIFIER_STRING, "net.babybluesheep.retro-2d-game");
        SDL.SDL_SetAppMetadataProperty(SDL.SDL_PROP_APP_METADATA_CREATOR_STRING, "babybluesheep");
        SDL.SDL_SetAppMetadataProperty(SDL.SDL_PROP_APP_METADATA_TYPE_STRING, "game");

        SDL.SDL_SetLogPriorities(SDL.SDL_LogPriority.SDL_LOG_PRIORITY_TRACE);

        var initFlags = SDL.SDL_InitFlags.SDL_INIT_VIDEO;
        if (!SDL.SDL_Init(initFlags))
        {
            SDL.SDL_LogError((int)SDL.SDL_LogCategory.SDL_LOG_CATEGORY_APPLICATION, $"Couldn't initialize SDL: {SDL.SDL_GetError()}");
            return;
        }

        //SDL.SDL_LogInfo(SDL.SDL_LogCategory.Application, $"VECTOR3 SIZE: {sizeof(Vector3)}");

        var windowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
        var window = new Window("Game", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, windowFlags);

        //SDL.SDL_SetRenderLogicalPresentation(renderer, GAME_WIDTH, GAME_HEIGHT, SDL.SDL_RendererLogicalPresentation.Letterbox);

        const int TICKS_PER_SECOND = 20;
        const int MAX_UPDATES_PER_TICK = 6;
        var gameEngine = new GameEngine(window, TimeSpan.FromSeconds(1.0 / TICKS_PER_SECOND), MAX_UPDATES_PER_TICK);

        gameEngine.GameStates.Push(new MainMenuState(gameEngine));

        gameEngine.Start();
        while (!gameEngine.HasRequestedToDie)
        {
            gameEngine.Run(window);
        }
        gameEngine.Stop();

        gameEngine.GraphicsDevice.UnclaimWindow(window);
        gameEngine.Dispose();

        window.Dispose();

        SDL.SDL_Quit();
    }
}
