using Game.Core.Game;
using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Content.GameStates;

internal class MainMenuSettingsState : GameState
{
    public enum SettingsCategory
    {
        None,
        Audio,
        Controls
    }

    private int _selectedOption;
    private SettingsCategory _selectedCategory;

    public MainMenuSettingsState(GameEngine gameEngine) : base(gameEngine)
    {
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

        int optionsAmount = _selectedCategory switch
        {
            SettingsCategory.None => 3,
            SettingsCategory.Audio => 3,
            SettingsCategory.Controls => 5,
            _ => 1,
        };

        if (_selectedOption < 0)
            _selectedOption = optionsAmount - 1;
        if (_selectedOption > optionsAmount - 1)
            _selectedOption = 0;

        if (GameEngine.Inputs.IsDown(InputButtonType.MenuConfirm))
        {
            switch (_selectedOption)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    GameEngine.GameStates.Pop();
                    break;
            }
        }
    }

    public override void FixedUpdate(TimeSpan delta)
    {

    }

    public override void Render(double progress)
    {
        /*
        if (_selectedOption == 0)
            SDL.SetRenderDrawColorFloat(renderer, 1f, 1f, 1f, SDL.AlphaOpaqueFloat);
        else
            SDL.SetRenderDrawColorFloat(renderer, 1f, 0f, 0f, SDL.AlphaOpaqueFloat);
        SDL.RenderDebugText(renderer, 15, 50, "Audio");
        if (_selectedOption == 1)
            SDL.SetRenderDrawColorFloat(renderer, 1f, 1f, 1f, SDL.AlphaOpaqueFloat);
        else
            SDL.SetRenderDrawColorFloat(renderer, 1f, 0f, 0f, SDL.AlphaOpaqueFloat);
        SDL.RenderDebugText(renderer, 15, 60, "Controls");
        if (_selectedOption == 2)
            SDL.SetRenderDrawColorFloat(renderer, 1f, 1f, 1f, SDL.AlphaOpaqueFloat);
        else
            SDL.SetRenderDrawColorFloat(renderer, 1f, 0f, 0f, SDL.AlphaOpaqueFloat);
        SDL.RenderDebugText(renderer, 15, 70, "Cancel");
        */
    }

    protected override void Dispose(bool disposing)
    {

    }
}