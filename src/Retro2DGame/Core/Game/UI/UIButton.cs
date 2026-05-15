using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Core.Game.UI;

internal enum UIButtonState
{
    Idle,
    Highlighted,
    Pressed,
}

internal sealed class UIButton
{

    private UIButtonState _state;
    public UIButtonState PreviousState { get; private set; }
    public UIButtonState State
    {
        get { return _state; }
        set
        {
            var didStateChange = _state != value;
            _state = value;
            if (didStateChange)
            {
                OnStateChange();
            }
        }
    }
    public RectangleF BoundingBox { get; set; }

    public Action OnStateChange { get; }

    public UIButton(Action onStateChange)
    {
        OnStateChange = onStateChange;
    }

    public void Update()
    {
        PreviousState = State;
    }

    public void ProcessMouseInputs(Vector2 mousePosition, bool isMouseDown)
    {
        if (!BoundingBox.Contains(mousePosition.X, mousePosition.Y))
        {
            State = UIButtonState.Idle;
            return;
        }
        if (!isMouseDown)
        {
            State = UIButtonState.Highlighted;
            return;
        }
        State = UIButtonState.Pressed;
    }
}
