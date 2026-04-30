using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using Retro2DGame.Core.SDL3.Extensions;
using SDL3;
using System.Drawing;

namespace Retro2DGame.Content.GameStates;

internal sealed class MainMenuState : GameState
{
    private TimeSpan _testTimer;

    private int _selectedOption;

    public MainMenuState(GameEngine engine) : base(engine)
    {
        _testTimer = TimeSpan.Zero;
    }

    public override void Update(TimeSpan delta)
    {
        _testTimer += delta;

        if (!ReferenceEquals(this, GameEngine.GameStates.Peek()))
            return;

        if (GameEngine.Inputs.IsDown(InputButtonType.MenuUp) && !GameEngine.Inputs.WasDown(InputButtonType.MenuUp))
        {
            _selectedOption--;
        }
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuDown) && !GameEngine.Inputs.WasDown(InputButtonType.MenuDown))
        {
            _selectedOption++;
        }

        if (_selectedOption < 0)
            _selectedOption = 2;
        if (_selectedOption > 2)
            _selectedOption = 0;

        if (GameEngine.Inputs.IsDown(InputButtonType.MenuConfirm))
        {
            switch (_selectedOption)
            {
                case 0:
                    GameEngine.GameStates.Pop();
                    GameEngine.GameStates.Push(new GameplayState(GameEngine));
                    break;
                case 1:
                    GameEngine.GameStates.Push(new MainMenuSettingsState(GameEngine));
                    break;
                case 2:
                    GameEngine.RequestToDie();
                    break;
            }
        }
    }

    public override void FixedUpdate(TimeSpan delta)
    {

    }

    public override void Render(double progress, PaletteIndexBitmap presentingBitmap)
    {
        GameEngine.AssetKeeper.RequestBitmap($"player_walk_{1 + (int)(_testTimer.TotalSeconds * 6) % 3}").Blit(presentingBitmap, 5, 5);
    }

    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            IsDisposed = true;
        }
    }
}