using Retro2DGame.Content.GameStates.MenuStates;
using Retro2DGame.Core.Game;
using SDL3;

namespace Retro2DGame;

internal sealed class Program
{
    static void Main(string[] args)
	{
		SDL.SetAppMetadataProperty(SDL.Props.AppMetadataNameString, "Game");
        SDL.SetAppMetadataProperty(SDL.Props.AppMetadataVersionString, "v1.0");
        SDL.SetAppMetadataProperty(SDL.Props.AppMetadataIdentifierString, "net.babybluesheep.game");
        SDL.SetAppMetadataProperty(SDL.Props.AppMetadataCreatorString, "babybluesheep");
        SDL.SetAppMetadataProperty(SDL.Props.AppMetadataTypeString, "game");

        SDL.SetLogPriorities(SDL.LogPriority.Trace);

        var initFlags = SDL.InitFlags.Video | SDL.InitFlags.Audio;
        if (!SDL.Init(initFlags))
		{
			SDL.LogError(SDL.LogCategory.Application, $"Couldn't initialize SDL: {SDL.GetError()}");
			return;
		}

        const int TICKS_PER_SECOND = 20;
		const int MAX_UPDATES_PER_TICK = 6;
        var gameEngine = new GameEngine(TimeSpan.FromSeconds(1.0 / TICKS_PER_SECOND), MAX_UPDATES_PER_TICK);

        gameEngine.GameStates.Push(new MainMenuBackgroundState(gameEngine));
        gameEngine.GameStates.Push(new MainMenuMainState(gameEngine));

        gameEngine.Start();
        while (!gameEngine.HasRequestedToDie)
		{
			gameEngine.Run();
        }
		gameEngine.Stop();

		gameEngine.Dispose();

		SDL.Quit();
    }
}

