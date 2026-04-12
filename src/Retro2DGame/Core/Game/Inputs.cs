using SDL3;

namespace Retro2DGame.Core.Game;

internal struct InputButtonState
{
    private bool _isDown;
    public readonly bool IsDown => _isDown;
    private bool _wasDown;
    public readonly bool WasDown => _wasDown;

    public InputButtonState()
    {
        _isDown = false;
        _wasDown = false;
    }

    public InputButtonState Update(bool isDown)
    {
        return new InputButtonState() { _isDown = isDown, _wasDown = _isDown };
    }
}

internal enum InputButtonType
{
    Up, Down, Left, Right,

    MenuUp, MenuDown, MenuLeft, MenuRight,
    MenuConfirm
}

internal sealed class Inputs
{
    private readonly Dictionary<InputButtonType, (HashSet<SDL.SDL_Scancode>, InputButtonState)> _buttonStates;

    public Inputs()
    {
        _buttonStates = [];

        _buttonStates.Add(InputButtonType.Up,           ([SDL.SDL_Scancode.SDL_SCANCODE_W],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.Left,         ([SDL.SDL_Scancode.SDL_SCANCODE_A],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.Right,        ([SDL.SDL_Scancode.SDL_SCANCODE_D],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.Down,         ([SDL.SDL_Scancode.SDL_SCANCODE_S],      new InputButtonState()));

        _buttonStates.Add(InputButtonType.MenuUp,       ([SDL.SDL_Scancode.SDL_SCANCODE_W],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.MenuLeft,     ([SDL.SDL_Scancode.SDL_SCANCODE_A],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.MenuRight,    ([SDL.SDL_Scancode.SDL_SCANCODE_D],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.MenuDown,     ([SDL.SDL_Scancode.SDL_SCANCODE_S],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.MenuConfirm,  ([SDL.SDL_Scancode.SDL_SCANCODE_RETURN], new InputButtonState()));
    }

    public void Reset()
    {
        foreach (var inputButtonType in _buttonStates.Keys)
        {
            var buttonState = _buttonStates[inputButtonType];

            buttonState.Item2 = new InputButtonState();

            _buttonStates[inputButtonType] = buttonState;
        }
    }

    public void Propagate()
    {
        foreach (var inputButtonType in _buttonStates.Keys)
        {
            var buttonState = _buttonStates[inputButtonType];

            buttonState.Item2 = buttonState.Item2.Update(buttonState.Item2.IsDown);

            _buttonStates[inputButtonType] = buttonState;
        }
    }

    public void UpdateEvent(SDL.SDL_Event @event)
    {
        if (@event.key.type == SDL.SDL_EventType.SDL_EVENT_KEY_DOWN)
        {
            foreach (var inputButtonType in _buttonStates.Keys)
            {
                var buttonState = _buttonStates[inputButtonType];

                if (buttonState.Item1.Contains(@event.key.scancode))
                {
                    buttonState.Item2 = buttonState.Item2.Update(true);
                }

                _buttonStates[inputButtonType] = buttonState;
            }
        }
        else if (@event.key.type == SDL.SDL_EventType.SDL_EVENT_KEY_UP)
        {
            foreach (var inputButtonType in _buttonStates.Keys)
            {
                var buttonState = _buttonStates[inputButtonType];

                if (buttonState.Item1.Contains(@event.key.scancode))
                {
                    buttonState.Item2 = buttonState.Item2.Update(false);
                }

                _buttonStates[inputButtonType] = buttonState;
            }
        }
    }

    public bool IsDown(InputButtonType button) => _buttonStates[button].Item2.IsDown;
    public bool WasDown(InputButtonType button) => _buttonStates[button].Item2.WasDown;
}
