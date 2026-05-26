using Retro2DGame.Core.NetExtensions;
using SDL3;
using System.Numerics;

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
    Pause,

    MenuUp, MenuDown, MenuLeft, MenuRight,
    MenuConfirm
}

internal sealed class Inputs
{
    private Vector2 _mousePosition;
    private Vector2 _previousMousePosition;
    public Vector2 MousePosition => _mousePosition;
    public Vector2 PreviousMousePosition => _previousMousePosition;
    private Vector2 _mouseWindowPosition;
    private Vector2 _previousMouseWindowPosition;
    public Vector2 WindowMousePosition => _mouseWindowPosition;
    public Vector2 WindowPreviousMousePosition => _previousMouseWindowPosition;
    public bool IsMouseLeftClickDown { get; private set; }
    public bool WasMouseLeftClickDown { get; private set; }
    public bool IsMouseRightClickDown { get; private set; }
    public bool WasMouseRightClickDown { get; private set; }


    private readonly Dictionary<InputButtonType, (HashSet<SDL.Scancode>, InputButtonState)> _buttonStates;

    public Inputs()
    {
        _buttonStates = [];

        _buttonStates.Add(InputButtonType.Up,           ([SDL.Scancode.W],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.Left,         ([SDL.Scancode.A],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.Right,        ([SDL.Scancode.D],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.Down,         ([SDL.Scancode.S],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.Pause,        ([SDL.Scancode.Escape], new InputButtonState()));

        _buttonStates.Add(InputButtonType.MenuUp,       ([SDL.Scancode.W],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.MenuLeft,     ([SDL.Scancode.A],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.MenuRight,    ([SDL.Scancode.D],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.MenuDown,     ([SDL.Scancode.S],      new InputButtonState()));
        _buttonStates.Add(InputButtonType.MenuConfirm,  ([SDL.Scancode.Return], new InputButtonState()));
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
        WasMouseLeftClickDown = IsMouseLeftClickDown;
        WasMouseRightClickDown = IsMouseRightClickDown;
        _previousMousePosition = _mousePosition;
        _previousMouseWindowPosition = _mouseWindowPosition;

        foreach (var inputButtonType in _buttonStates.Keys)
        {
            var buttonState = _buttonStates[inputButtonType];

            buttonState.Item2 = buttonState.Item2.Update(buttonState.Item2.IsDown);

            _buttonStates[inputButtonType] = buttonState;
        }
    }

    public void UpdateEvent(SDL.Event @event, Vector2 windowSize, Vector2 gameSize)
    {
        switch (@event.Key.Type)
        {
            case SDL.EventType.KeyDown:
            case SDL.EventType.KeyUp:
                var isDown = (SDL.EventType)@event.Type == SDL.EventType.KeyDown;
                foreach (var inputButtonType in _buttonStates.Keys)
                {
                    var buttonState = _buttonStates[inputButtonType];

                    if (buttonState.Item1.Contains(@event.Key.Scancode))
                    {
                        buttonState.Item2 = buttonState.Item2.Update(isDown);
                    }

                    _buttonStates[inputButtonType] = buttonState;
                }
                break;

            case SDL.EventType.MouseMotion:
                _mousePosition.X = @event.Motion.X;
                _mousePosition.Y = @event.Motion.Y;

                _mouseWindowPosition = _mousePosition;
                _mousePosition = Vector2.RemapLetterboxed(_mousePosition, windowSize, gameSize);

                break;

            case SDL.EventType.MouseButtonDown:
                if (@event.Button.Button == SDL.ButtonLeft)
                    IsMouseLeftClickDown = true;
                if (@event.Button.Button == SDL.ButtonRight)
                    IsMouseRightClickDown = true;
                break;
            case SDL.EventType.MouseButtonUp:
                if (@event.Button.Button == SDL.ButtonLeft)
                    IsMouseLeftClickDown = false;
                if (@event.Button.Button == SDL.ButtonRight)
                    IsMouseRightClickDown = false;
                break;

            default:
                break;
        }
    }

    public bool IsDown(InputButtonType button) => _buttonStates[button].Item2.IsDown;
    public bool WasDown(InputButtonType button) => _buttonStates[button].Item2.WasDown;
}
