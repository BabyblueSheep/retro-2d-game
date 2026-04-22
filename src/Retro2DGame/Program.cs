using Retro2DGame.Content.GameStates;
using Retro2DGame.Core;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using SDL3;

namespace Retro2DGame;

internal sealed class Program
{
	public const int DEFAULT_WINDOW_WIDTH = 640;
	public const int DEFAULT_WINDOW_HEIGHT = 480;

	public const int GAME_WIDTH = 256;
	public const int GAME_HEIGHT = 256;

    static void Main(string[] args)
	{
		SDL.SetAppMetadataProperty(SDL.Props.AppMetadataNameString, "Game");
        SDL.SetAppMetadataProperty(SDL.Props.AppMetadataVersionString, "v1.0");
        SDL.SetAppMetadataProperty(SDL.Props.AppMetadataIdentifierString, "net.babybluesheep.game");
        SDL.SetAppMetadataProperty(SDL.Props.AppMetadataCreatorString, "babybluesheep");
        SDL.SetAppMetadataProperty(SDL.Props.AppMetadataTypeString, "game");

        SDL.SetLogPriorities(SDL.LogPriority.Trace);

        var initFlags = SDL.InitFlags.Video;
        if (!SDL.Init(initFlags))
		{
			SDL.LogError(SDL.LogCategory.Application, $"Couldn't initialize SDL: {SDL.GetError()}");
			return;
		}

        var windowFlags = SDL.WindowFlags.Resizable;
		var window = new Window("Game", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, windowFlags);
        var renderer = Renderer.Create(window, "software");

        SDL.SetRenderLogicalPresentation(renderer.Handle, GAME_WIDTH, GAME_HEIGHT, SDL.RendererLogicalPresentation.Letterbox);

        const int TICKS_PER_SECOND = 20;
		const int MAX_UPDATES_PER_TICK = 6;
        var gameEngine = new GameEngine(TimeSpan.FromSeconds(1.0 / TICKS_PER_SECOND), MAX_UPDATES_PER_TICK);

        gameEngine.GameStates.Push(new MainMenuState(renderer, gameEngine));

        gameEngine.Start();
        while (!gameEngine.HasRequestedToDie)
		{
			gameEngine.Run(window, renderer);
        }
		gameEngine.Stop();

		gameEngine.Dispose();

        renderer.Dispose();
        window.Dispose();

		SDL.Quit();
    }
}

