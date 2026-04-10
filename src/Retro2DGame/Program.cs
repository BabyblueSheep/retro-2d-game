using MoonWorks;

namespace Retro2DGame;

internal sealed class Program
{
    static void Main(string[] args)
    {
        AppInfo appInfo = new
        (
            OrganizationName: "babybluesheep",
            ApplicationName: "Retro 2D Game"
        );

        WindowCreateInfo windowCreateInfo = new
        (
            windowTitle: "Bullet Game Prototype",
            windowWidth: 640,
            windowHeight: 480,
            screenMode: ScreenMode.Windowed,
            systemResizable: true,
            startMaximized: false,
            highDPI: false
        );

        FramePacingSettings framePacingSettings = FramePacingSettings.CreateCapped(60, 6);

        var game = new BulletGame(appInfo, windowCreateInfo, framePacingSettings);
        game.Run();
    }
}
