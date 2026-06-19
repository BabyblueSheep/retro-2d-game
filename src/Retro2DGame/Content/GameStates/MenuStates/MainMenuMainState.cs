using Retro2DGame.Core;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Audio;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.Game.UI;
using Retro2DGame.Core.NetExtensions;
using Retro2DGame.Core.SDLWrappers;
using Retro2DGame.Core.SDLWrappers.Extensions;
using SDL3;
using System.Drawing;
using System.Numerics;
using static Retro2DGame.Core.Game.Audio.AudioPlayer;

namespace Retro2DGame.Content.GameStates.MenuStates;

internal sealed class MainMenuMainState : GameState
{
    private readonly UIButtonGroup _buttonGroup;

    private readonly ConsistentRandom _random;
    private IndependentAudioHandle? _audioHandle;

    public MainMenuMainState(GameEngine engine) : base(engine)
    {
        _buttonGroup = new UIButtonGroup
        (
            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
                    {
                        GameEngine.GameStates.Pop();
                        GameEngine.GameStates.Push(new MainMenuLevelSelectState(GameEngine));
                    }
                }
            )
            {
                Text = "Play",
                Dimensions = new RectangleF(176, 208, TextRenderer.GetTextWidth("Settings"), TextRenderer.GetTextHeight()),
                Margin = new Vector2(4, 4),
                TextAlignment = TextRenderer.TextAlignment.Right,
            },

            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
                    {
                        GameEngine.GameStates.Pop();
                        GameEngine.GameStates.Push(new MainMenuSettingsState(GameEngine));
                    }
                }
            )
            {
                Text = "Settings",
                Dimensions = new RectangleF(176, 224, TextRenderer.GetTextWidth("Settings"), TextRenderer.GetTextHeight()),
                Margin = new Vector2(4, 4),
                TextAlignment = TextRenderer.TextAlignment.Right,
            },

            new UITextButton(
                (UIButton button) =>
                {
                    if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
                    {
                        GameEngine.RequestToDie();
                    }
                }
            )
            {
                Text = "Quit",
                Dimensions = new RectangleF(176, 240, TextRenderer.GetTextWidth("Settings"), TextRenderer.GetTextHeight()),
                Margin = new Vector2(4, 4),
                TextAlignment = TextRenderer.TextAlignment.Right,
            }
        );

        
       _random = new ConsistentRandom(1);
    }

    public override void Enter()
    {
        GameEngine.Logger.LogDebug($"Entering game state {nameof(MainMenuMainState)}");
    }

    public override void Exit()
    {
        GameEngine.Logger.LogDebug($"Exiting game state {nameof(MainMenuMainState)}");
    }

    public override void Update(TimeSpan delta)
    {
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuUp) && !GameEngine.Inputs.WasDown(InputButtonType.MenuUp))
        {
            _buttonGroup.SelectedIndex--;
        }
        if (GameEngine.Inputs.IsDown(InputButtonType.MenuDown) && !GameEngine.Inputs.WasDown(InputButtonType.MenuDown))
        {
            _buttonGroup.SelectedIndex++;
        }
        _buttonGroup.SelectedIndex = int.Wrap(_buttonGroup.SelectedIndex, 3);

        _buttonGroup.PropagateState();

        _buttonGroup.ProcessButtons
        (
            GameEngine.Inputs.IsDown(InputButtonType.MenuConfirm), GameEngine.Inputs.WasDown(InputButtonType.MenuConfirm),
            GameEngine.Inputs.MousePosition, GameEngine.Inputs.IsMouseLeftClickDown,
            GameEngine.Inputs.PreviousMousePosition, GameEngine.Inputs.WasMouseLeftClickDown
        );


        
        if (GameEngine.Inputs.IsMouseLeftClickDown && !GameEngine.Inputs.WasMouseLeftClickDown)
        {
            GameEngine.SoundPlayer.Play(GameEngine.AssetStorage.Audio.Hurt3, new AudioPlayParameters() with 
            { 
                Pitch = _random.RandomFloat(0.8f, 1.2f) 
            });
        }
        if (GameEngine.Inputs.IsMouseRightClickDown && !GameEngine.Inputs.WasMouseRightClickDown)
        {
            if (_audioHandle != null)
            {
                _audioHandle.Stop();
                _audioHandle = null;
            }
            else
            {
                _audioHandle = GameEngine.SoundPlayer.Play(GameEngine.AssetStorage.Audio.Hurt3, new AudioPlayParameters() with 
                { 
                    Pitch = _random.RandomFloat(0.3f, 0.5f),
                    ShouldLoop = true
                });
            }
        }
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
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            IsDisposed = true;
        }
    }
}