using Retro2DGame.Core.Game;

namespace Retro2DGame.Content.GameStates;

internal sealed class MainMenuState : GameState
{
    private int _selectedOption;

    public MainMenuState(GameEngine engine) : base(engine)
    {

    }

    public override void Update(TimeSpan delta)
    {
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

    public override void Render(double progress)
    {
        /*
        SDL.SetRenderDrawColorFloat(renderer, 0f, 0f, 0f, SDL.AlphaOpaqueFloat);
        SDL.RenderClear(renderer);

        SDL.SetRenderDrawColorFloat(renderer, 1f, 0f, 0f, SDL.AlphaOpaqueFloat);
        SDL.RenderRect(renderer, new SDL.FRect()
        {
            X = 0,
            Y = 0,
            W = Program.GAME_WIDTH,
            H = Program.GAME_HEIGHT
        });

        if (ReferenceEquals(this, GameEngine.GameStates.Peek()))
        {
            SDL.SetRenderDrawColorFloat(renderer, 1f, 0f, 0f, SDL.AlphaOpaqueFloat);
            SDL.SetRenderScale(renderer, 2f, 2f);
            SDL.RenderDebugText(renderer, 5, 5, "Game");

            SDL.SetRenderScale(renderer, 1f, 1f);
            if (_selectedOption == 0)
                SDL.SetRenderDrawColorFloat(renderer, 1f, 1f, 1f, SDL.AlphaOpaqueFloat);
            else
                SDL.SetRenderDrawColorFloat(renderer, 1f, 0f, 0f, SDL.AlphaOpaqueFloat);
            SDL.RenderDebugText(renderer, 15, 50, "Start");
            if (_selectedOption == 1)
                SDL.SetRenderDrawColorFloat(renderer, 1f, 1f, 1f, SDL.AlphaOpaqueFloat);
            else
                SDL.SetRenderDrawColorFloat(renderer, 1f, 0f, 0f, SDL.AlphaOpaqueFloat);
            SDL.RenderDebugText(renderer, 15, 60, "Options");
            if (_selectedOption == 2)
                SDL.SetRenderDrawColorFloat(renderer, 1f, 1f, 1f, SDL.AlphaOpaqueFloat);
            else
                SDL.SetRenderDrawColorFloat(renderer, 1f, 0f, 0f, SDL.AlphaOpaqueFloat);
            SDL.RenderDebugText(renderer, 15, 70, "End");
        }
        */
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