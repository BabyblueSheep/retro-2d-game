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
        
    }

    public override void Exit()
    {
        
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
            GameEngine.AssetStorage.Background.Generic,
            0, 0
        );
    }

    protected override void Dispose(bool disposing)
    {
        
    }
}
