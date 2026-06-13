using Retro2DGame.Content.GameStates.MenuStates;
using Retro2DGame.Core.Game;

namespace Retro2DGame;

internal sealed class Program
{
    static void Main(string[] args)
	{
        SDL3.SDL.SetAppMetadataProperty(SDL3.SDL.Props.AppMetadataNameString, "Game");
        SDL3.SDL.SetAppMetadataProperty(SDL3.SDL.Props.AppMetadataVersionString, "v1.0");
        SDL3.SDL.SetAppMetadataProperty(SDL3.SDL.Props.AppMetadataIdentifierString, "net.babybluesheep.game");
        SDL3.SDL.SetAppMetadataProperty(SDL3.SDL.Props.AppMetadataCreatorString, "babybluesheep");
        SDL3.SDL.SetAppMetadataProperty(SDL3.SDL.Props.AppMetadataTypeString, "game");

        var logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        var logFilePath = Path.Combine(logDirectoryPath, "log.txt");
        Directory.CreateDirectory(logDirectoryPath);
        File.WriteAllText(logFilePath, "");

        var logger = new Logger();
        logger.EnableConsoleLogging();
        logger.EnableFileLogging(Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.txt"));
        logger.LogPriority = Logger.LogPriorityLevel.Debug;

        logger.LogInfo($"Using SDL {SDL3.SDL.VersionNumMajor(SDL3.SDL.GetVersion())}.{SDL3.SDL.VersionNumMinor(SDL3.SDL.GetVersion())}.{SDL3.SDL.VersionNumMicro(SDL3.SDL.GetVersion())}");

        var initFlags = SDL3.SDL.InitFlags.Video | SDL3.SDL.InitFlags.Audio;
        if (!SDL3.SDL.Init(initFlags))
		{
            logger.LogError($"Couldn't initialize SDL: {SDL3.SDL.GetError()}");
			return;
		}

        logger.LogInfo($"SDL initialized!");

        logger.LogInfo($"Using SDL_mixer {SDL3.SDL.VersionNumMajor(SDL3.Mixer.Version())}.{SDL3.SDL.VersionNumMinor(SDL3.Mixer.Version())}.{SDL3.SDL.VersionNumMicro(SDL3.Mixer.Version())}");

        if (!SDL3.Mixer.Init())
        {
            logger.LogError($"Couldn't initialize SDL_mixer: {SDL3.SDL.GetError()}");

            logger.LogInfo("Terminating program");
            SDL3.SDL.Quit();

            return;
        }

        logger.LogInfo($"SDL_mixer initialized!");

        const int TICKS_PER_SECOND = 20;
		const int MAX_UPDATES_PER_TICK = 6;
        logger.LogDebug($"Targetting {TICKS_PER_SECOND} ticks per second, with up to {MAX_UPDATES_PER_TICK} ticks in one frame");

        try
        {
            var gameEngine = new GameEngine
            (
                TimeSpan.FromSeconds(1.0 / TICKS_PER_SECOND), MAX_UPDATES_PER_TICK,
                logFilePath
            );

            gameEngine.GameStates.Push(new MainMenuBackgroundState(gameEngine));
            gameEngine.GameStates.Push(new MainMenuMainState(gameEngine));

            gameEngine.Start();
            while (!gameEngine.HasRequestedToDie)
            {
                gameEngine.Run();
            }
            gameEngine.Stop();

            gameEngine.Dispose();
        }
        catch (Exception exception)
        {
            logger.LogError(exception.ToString());
            Environment.FailFast(null);
        }

        logger.LogInfo("Terminating program");
        SDL3.SDL.Quit();
    }
}

