using Retro2DGame.Core.NetExtensions;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.Game.UI;
using Retro2DGame.Core.SDL3;
using Retro2DGame.Core.SDL3.Extensions;
using SDL3;
using System.Drawing;
using System.Numerics;

namespace Retro2DGame.Content.GameStates;

internal sealed class MainMenuState : GameState
{
    private readonly UIButtonGroup _buttonGroup;

    public MainMenuState(GameEngine engine) : base(engine)
    {
        _buttonGroup = new UIButtonGroup
        (
            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState == UIButton.ButtonState.Highlighted && !_buttonGroup!.HasAnyButtonBeenPressedThisMousePress)
                        SDL.LogInfo(SDL.LogCategory.Application, "Play");
                }
            )
            {
                OriginalBoundingBox = new RectangleF(176, 192, 1, 1),
                Text = "Play",
                Margin = new Vector2(4, 4)
            },

            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState == UIButton.ButtonState.Highlighted && !_buttonGroup!.HasAnyButtonBeenPressedThisMousePress)
                        SDL.LogInfo(SDL.LogCategory.Application, "Settings");
                }
            )
            {
                OriginalBoundingBox = new RectangleF(176, 208, 1, 1),
                Text = "Settings",
                Margin = new Vector2(4, 4)
            },

            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState == UIButton.ButtonState.Highlighted && !_buttonGroup!.HasAnyButtonBeenPressedThisMousePress)
                        SDL.LogInfo(SDL.LogCategory.Application, "Quit");
                }
            )
            {
                OriginalBoundingBox = new RectangleF(176, 224, 1, 1),
                Text = "Quit",
                Margin = new Vector2(4, 4)
            }
        );
    }

    public override void Update(TimeSpan delta)
    {
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuUp) && !GameEngine.Inputs.WasDown(InputButtonType.MenuUp))
        {
            _buttonGroup.DecrementSelectedIndex();
        }
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuDown) && !GameEngine.Inputs.WasDown(InputButtonType.MenuDown))
        {
            _buttonGroup.IncrementSelectedIndex();
        }

        _buttonGroup.PropagateState();

        _buttonGroup.ProcessButtons
        (
            GameEngine.Inputs.IsDown(InputButtonType.MenuConfirm),
            GameEngine.Inputs.MousePosition, GameEngine.Inputs.IsMouseLeftClickDown,
            GameEngine.Inputs.PreviousMousePosition, GameEngine.Inputs.WasMouseLeftClickDown
        );

        /*if (GameEngine.Inputs.IsDown(InputButtonType.MenuConfirm))
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
        }*/
    }

    public override void FixedUpdate(TimeSpan delta)
    {

    }

    public override void Render(double progress)
    {
        _buttonGroup.Render(GameEngine.AssetStorage, GameEngine.ForegroundBitmap);
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