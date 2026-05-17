using Retro2DGame.Core.Extensions;
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
    private int _selectedOption = 0;

    private readonly UIButton _menuButtonPlay;
    private readonly UIButton _menuButtonSettings;
    private readonly UIButton _menuButtonQuit;

    public MainMenuState(GameEngine engine) : base(engine)
    {
        _menuButtonPlay = new UIButton(() =>
        {
            if (_menuButtonPlay!.State != UIButtonState.Idle)
                _selectedOption = 0;

            if (_menuButtonPlay!.State == UIButtonState.Pressed && _menuButtonPlay.PreviousState != UIButtonState.Pressed)
                SDL.LogInfo(SDL.LogCategory.Application, "Play");
        })
        {
            BoundingBox = RectangleF.Inflate(new Rectangle(176, 192, TextRenderer.GetTextWidth("Play"), TextRenderer.GetTextHeight()), 4, 4)
        };

        _menuButtonSettings = new UIButton(() =>
        {
            if (_menuButtonSettings!.State != UIButtonState.Idle)
                _selectedOption = 1;

            if (_menuButtonSettings!.State == UIButtonState.Pressed && _menuButtonSettings.PreviousState != UIButtonState.Pressed)
                SDL.LogInfo(SDL.LogCategory.Application, "Settings");
        })
        {
            BoundingBox = RectangleF.Inflate(new Rectangle(176, 208, TextRenderer.GetTextWidth("Settings"), TextRenderer.GetTextHeight()), 4, 4)
        };

        _menuButtonQuit = new UIButton(() => 
        {
            if (_menuButtonQuit!.State != UIButtonState.Idle)
                _selectedOption = 2;

            if (_menuButtonQuit!.State == UIButtonState.Pressed && _menuButtonQuit.PreviousState != UIButtonState.Pressed)
                SDL.LogInfo(SDL.LogCategory.Application, "Quit");
        })
        {
            BoundingBox = RectangleF.Inflate(new Rectangle(176, 224, TextRenderer.GetTextWidth("Quit"), TextRenderer.GetTextHeight()), 4, 4)
        };
    }

    public override void Update(TimeSpan delta)
    {
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuUp) && !GameEngine.Inputs.WasDown(InputButtonType.MenuUp))
        {
            _selectedOption--;
        }
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuDown) && !GameEngine.Inputs.WasDown(InputButtonType.MenuDown))
        {
            _selectedOption++;
        }

        _selectedOption = (_selectedOption + 3) % 3;

        var shouldUseMouseForButtonInputs = false;
        shouldUseMouseForButtonInputs |= GameEngine.Inputs.IsMouseLeftClickDown != GameEngine.Inputs.WasMouseLeftClickDown;
        shouldUseMouseForButtonInputs |= GameEngine.Inputs.MousePosition != GameEngine.Inputs.PreviousMousePosition;

        if (shouldUseMouseForButtonInputs)
        {
            shouldUseMouseForButtonInputs = false;
            shouldUseMouseForButtonInputs |= _menuButtonPlay.BoundingBox.Contains(GameEngine.Inputs.MousePosition.X, GameEngine.Inputs.MousePosition.Y);
            shouldUseMouseForButtonInputs |= _menuButtonSettings.BoundingBox.Contains(GameEngine.Inputs.MousePosition.X, GameEngine.Inputs.MousePosition.Y);
            shouldUseMouseForButtonInputs |= _menuButtonQuit.BoundingBox.Contains(GameEngine.Inputs.MousePosition.X, GameEngine.Inputs.MousePosition.Y);
        }

        if (shouldUseMouseForButtonInputs)
        {
            _menuButtonPlay.ProcessMouseInputs(GameEngine.Inputs.MousePosition, GameEngine.Inputs.IsMouseLeftClickDown);
            _menuButtonSettings.ProcessMouseInputs(GameEngine.Inputs.MousePosition, GameEngine.Inputs.IsMouseLeftClickDown);
            _menuButtonQuit.ProcessMouseInputs(GameEngine.Inputs.MousePosition, GameEngine.Inputs.IsMouseLeftClickDown);
        }
        else
        {
            var isPressingButton = GameEngine.Inputs.IsDown(InputButtonType.MenuConfirm);
            switch (_selectedOption)
            {
                case 0:
                    _menuButtonPlay.State = isPressingButton ? UIButtonState.Pressed : UIButtonState.Highlighted;
                    _menuButtonSettings.State = UIButtonState.Idle;
                    _menuButtonQuit.State = UIButtonState.Idle;
                    break;
                case 1:
                    _menuButtonPlay.State = UIButtonState.Idle;
                    _menuButtonSettings.State = isPressingButton ? UIButtonState.Pressed : UIButtonState.Highlighted;
                    _menuButtonQuit.State = UIButtonState.Idle;
                    break;
                case 2:
                    _menuButtonPlay.State = UIButtonState.Idle;
                    _menuButtonSettings.State = UIButtonState.Idle;
                    _menuButtonQuit.State = isPressingButton ? UIButtonState.Pressed : UIButtonState.Highlighted;
                    break;
            }
        }

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
        var playText = "Play";
        if (_menuButtonPlay.State == UIButtonState.Highlighted)
        {
            playText = string.Concat("{", playText, "}");
        }
        GameEngine.TextRenderer.BlitTextDefault
        (
            GameEngine.ForegroundBitmap,
            176, 192,
            playText
        );

        var settingsText = "Settings";
        if (_menuButtonSettings.State == UIButtonState.Highlighted)
        {
            settingsText = string.Concat("{", settingsText, "}");
        }
        GameEngine.TextRenderer.BlitTextDefault
        (
            GameEngine.ForegroundBitmap,
            176, 208,
            settingsText
        );

        var quitText = "Quit";
        if (_menuButtonQuit.State == UIButtonState.Highlighted)
        {
            quitText = string.Concat("{", quitText, "}");
        }
        GameEngine.TextRenderer.BlitTextDefault
        (
            GameEngine.ForegroundBitmap,
            176, 224,
            quitText
        );
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