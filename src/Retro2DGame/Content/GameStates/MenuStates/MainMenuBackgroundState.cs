using Retro2DGame.Core.Game;
using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Content.GameStates.MenuStates;

internal sealed class MainMenuBackgroundState : GameState
{
    public MainMenuBackgroundState(GameEngine gameEngine) : base(gameEngine)
    {

    }

    public override void Enter()
    {
        GameEngine.Logger.LogDebug($"Entering game state {nameof(MainMenuBackgroundState)}");
    }

    public override void Exit()
    {
        GameEngine.Logger.LogDebug($"Entering game state {nameof(MainMenuBackgroundState)}");
    }

    public override void Update(TimeSpan delta)
    {
        
    }

    public override void FixedUpdate(TimeSpan delta)
    {

    }

    public override void Render(double progress)
    {
        GameEngine.Bitmap.Blit
        (
            GameEngine.AssetStorage.Sprites.Background.Generic,
            0, 0
        );
    }

    protected override void Dispose(bool disposing)
    {
        
    }
}
