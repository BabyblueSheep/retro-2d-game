using Retro2DGame.Content.GameStates.GameplayStates;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.Game.UI;
using Retro2DGame.Core.NetExtensions;
using SDL3;
using System.Drawing;
using System.Numerics;

namespace Retro2DGame.Content.GameStates.MenuStates;

internal sealed class MainMenuLevelSelectState : GameState
{
    private readonly UIButtonGroup _buttonGroup;

    private int _levelToGoTo = -1;

    public MainMenuLevelSelectState(GameEngine gameEngine) : base(gameEngine)
    {
        _buttonGroup = new UIButtonGroup
        (
            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
                    {
                        _levelToGoTo = 1;
                        //GameEngine.GameStates.Pop();
                    }
                }
            )
            {
                Text = "Level 1",
                Dimensions = new RectangleF(128 - 96 - TextRenderer.GetTextWidth("Level 1") / 2, 96, TextRenderer.GetTextWidth("Level 1"), TextRenderer.GetTextHeight()),
                Margin = new Vector2(4, 4),
                TextAlignment = TextRenderer.TextAlignment.Center,
            },

            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
                    {
                        _levelToGoTo = 2;
                        //GameEngine.GameStates.Pop();
                    }
                }
            )
            {
                Text = "Level 2",
                Dimensions = new RectangleF(128 - TextRenderer.GetTextWidth("Level 1") / 2, 96, TextRenderer.GetTextWidth("Level 1"), TextRenderer.GetTextHeight()),
                Margin = new Vector2(4, 4),
                TextAlignment = TextRenderer.TextAlignment.Center,
            },

            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
                    {
                        _levelToGoTo = 3;
                        //GameEngine.GameStates.Pop();
                    }
                }
            )
            {
                Text = "Level 3",
                Dimensions = new RectangleF(128 + 96 - TextRenderer.GetTextWidth("Level 1") / 2, 96, TextRenderer.GetTextWidth("Level 1"), TextRenderer.GetTextHeight()),
                Margin = new Vector2(4, 4),
                TextAlignment = TextRenderer.TextAlignment.Center,
            },

            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
                    {
                        _levelToGoTo = 4;
                        GameEngine.GameStates.Pop();
                    }
                }
            )
            {
                Text = "Endless",
                Dimensions = new RectangleF(128 - TextRenderer.GetTextWidth("Endless") / 2, 160, TextRenderer.GetTextWidth("Endless"), TextRenderer.GetTextHeight()),
                Margin = new Vector2(4, 4),
                TextAlignment = TextRenderer.TextAlignment.Center,
            },

            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
                    {
                        _levelToGoTo = -1;
                        GameEngine.GameStates.Pop();
                        GameEngine.GameStates.Push(new MainMenuMainState(GameEngine));
                    }
                }
            )
            {
                Text = "Exit",
                Dimensions = new RectangleF(8, 240, TextRenderer.GetTextWidth("Exit"), TextRenderer.GetTextHeight()),
                Margin = new Vector2(4, 4),
                TextAlignment = TextRenderer.TextAlignment.Left,
            }
        );
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        if (_levelToGoTo == -1)
            return;

        GameEngine.GameStates.Pop();

        if (_levelToGoTo == 4)
        {
            GameEngine.GameStates.Push(new EndlessGameplayState(GameEngine));
        }
        else
        {
            GameEngine.GameStates.Push(new LevelGameplayState(GameEngine));
        }
    }

    public override void Update(TimeSpan delta)
    {
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuLeft) && !GameEngine.Inputs.WasDown(InputButtonType.MenuLeft))
        {
            _buttonGroup.SelectedIndex--;
            _buttonGroup.SelectedIndex = int.Wrap(_buttonGroup.SelectedIndex, 3);
        }
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuRight) && !GameEngine.Inputs.WasDown(InputButtonType.MenuRight))
        {
            _buttonGroup.SelectedIndex++;
            _buttonGroup.SelectedIndex = int.Wrap(_buttonGroup.SelectedIndex, 3);
        }

        if (GameEngine.Inputs.IsDown(InputButtonType.MenuDown) && !GameEngine.Inputs.WasDown(InputButtonType.MenuDown))
        {
            if (_buttonGroup.SelectedIndex == 3)
            {
                _buttonGroup.SelectedIndex = 4;
            }
            else if (_buttonGroup.SelectedIndex == 4)
            {
                _buttonGroup.SelectedIndex = 0;
            }
            else
            {
                _buttonGroup.SelectedIndex = 3;
            }
        }

        if (GameEngine.Inputs.IsDown(InputButtonType.MenuUp) && !GameEngine.Inputs.WasDown(InputButtonType.MenuUp))
        {
            if (_buttonGroup.SelectedIndex == 3)
            {
                _buttonGroup.SelectedIndex = 0;
            }
            else if (_buttonGroup.SelectedIndex == 4)
            {
                _buttonGroup.SelectedIndex = 3;
            }
            else
            {
                _buttonGroup.SelectedIndex = 4;
            }
        }

        _buttonGroup.PropagateState();

        _buttonGroup.ProcessButtons
        (
            GameEngine.Inputs.IsDown(InputButtonType.MenuConfirm), GameEngine.Inputs.WasDown(InputButtonType.MenuConfirm),
            GameEngine.Inputs.MousePosition, GameEngine.Inputs.IsMouseLeftClickDown,
            GameEngine.Inputs.PreviousMousePosition, GameEngine.Inputs.WasMouseLeftClickDown
        );
    }

    public override void FixedUpdate(TimeSpan delta)
    {

    }

    public override void Render(double progress)
    {
        _buttonGroup.Render(GameEngine.AssetStorage, GameEngine.Bitmap);
    }

    protected override void Dispose(bool disposing)
    {
        
    }
}
