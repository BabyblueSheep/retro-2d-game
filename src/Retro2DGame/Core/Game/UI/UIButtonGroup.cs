using Retro2DGame.Core.Game.Rendering;
using System.Numerics;

namespace Retro2DGame.Core.Game.UI;

internal sealed class UIButtonGroup
{
    private readonly UIButton[] _buttons;

    private int _currentlySelectedIndex;

    public UIButtonGroup(params UIButton[] buttons)
    {
        _buttons = buttons;
        _currentlySelectedIndex = 0;
    }

    private void WrapSelectedIndex()
    {
        while (_currentlySelectedIndex < 0)
        {
            _currentlySelectedIndex += _buttons.Length;
        }
        _currentlySelectedIndex %= _buttons.Length;
    }

    public void IncrementSelectedIndex(int amount = 1)
    {
        _currentlySelectedIndex += amount;
        WrapSelectedIndex();
    }

    public void DecrementSelectedIndex(int amount = 1)
    {
        _currentlySelectedIndex -= amount;
        WrapSelectedIndex();
    }

    private void TryForceSelectedIndex(int index)
    {
        var button = _buttons[index];
        if (button.State != UIButton.ButtonState.Idle)
        {
            _currentlySelectedIndex = index;
        }
    }

    private void ProcessButtonMouse
    (
        int index,
        Vector2 mousePosition, bool isMouseDown,
        Vector2 previousMousePosition, bool wasMouseDown
    )
    {
        var button = _buttons[index];

        if (!button.BoundingBox.Contains(mousePosition.X, mousePosition.Y))
        {
            button.State = UIButton.ButtonState.Idle;
            return;
        }

        var isMouseCurrentlyHoldingButton = button.BoundingBox.Contains(mousePosition.X, mousePosition.Y) && isMouseDown;

        if (isMouseCurrentlyHoldingButton && (!wasMouseDown || button.PreviousState == UIButton.ButtonState.Held))
        {
            button.State = UIButton.ButtonState.Held;
        }
        else
        {
            button.State = UIButton.ButtonState.Highlighted;
        }
    }

    private void ProcessButtonManual
    (
        int index,
        bool isManuallyPressing, bool wasManuallyPressing
    )
    {
        var button = _buttons[index];

        if (index == _currentlySelectedIndex)
        {
            button.State = (isManuallyPressing && (!wasManuallyPressing || button.PreviousState == UIButton.ButtonState.Held)) ? UIButton.ButtonState.Held : UIButton.ButtonState.Highlighted;
        }
        else
        {
            button.State = UIButton.ButtonState.Idle;
        }
    }

    public void PropagateState()
    {
        foreach (var button in _buttons)
        {
            button.PropagateState();
        }
    }

    public void ProcessButtons
    (
        bool isManuallyPressing, bool wasManuallyPressing,
        Vector2 mousePosition, bool isMouseDown,
        Vector2 previousMousePosition, bool wasMouseDown
    )
    {
        var shouldUseMouseForButtonInputs = isMouseDown;
        shouldUseMouseForButtonInputs |= isMouseDown != wasMouseDown;
        shouldUseMouseForButtonInputs |= mousePosition != previousMousePosition;

        if (shouldUseMouseForButtonInputs)
        {
            shouldUseMouseForButtonInputs = false;
            foreach (var button in _buttons)
            {
                shouldUseMouseForButtonInputs |= button.BoundingBox.Contains(mousePosition.X, mousePosition.Y);
            }
        }

        if (shouldUseMouseForButtonInputs)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                ProcessButtonMouse(i, mousePosition, isMouseDown, previousMousePosition, wasMouseDown);
                TryForceSelectedIndex(i);
            }
        }
        else
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                ProcessButtonManual(i, isManuallyPressing, wasManuallyPressing);
                TryForceSelectedIndex(i);
            }
        }
    }

    public void Render(AssetStorage assets, PaletteIndexBitmap destination)
    {
        foreach (var button in _buttons)
        {
            button.Render(assets, destination);
        }
    }
}
