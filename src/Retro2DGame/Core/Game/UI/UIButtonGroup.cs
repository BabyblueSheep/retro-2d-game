using Retro2DGame.Core.Game.Rendering;
using System.Numerics;

namespace Retro2DGame.Core.Game.UI;

internal sealed class UIButtonGroup
{
    private readonly UIButton[] _buttons;

    public int CurrentlySelectedIndex { get; private set; }
    public bool HasAnyButtonBeenPressedThisMousePress { get; private set; }

    public UIButtonGroup(params UIButton[] buttons)
    {
        _buttons = buttons;
        CurrentlySelectedIndex = 0;
    }

    private void WrapSelectedIndex()
    {
        CurrentlySelectedIndex = (CurrentlySelectedIndex + _buttons.Length) % _buttons.Length;
    }

    public void IncrementSelectedIndex()
    {
        CurrentlySelectedIndex++;
        WrapSelectedIndex();
    }

    public void DecrementSelectedIndex()
    {
        CurrentlySelectedIndex--;
        WrapSelectedIndex();
    }

    private void TryForceSelectedIndex(int index)
    {
        var button = _buttons[index];
        if (button.State != UIButton.ButtonState.Idle)
        {
            CurrentlySelectedIndex = index;
        }
    }

    private void TryMarkAnyButtonAsPressed(int index)
    {
        var button = _buttons[index];
        if (button.State == UIButton.ButtonState.Held && button.PreviousState != UIButton.ButtonState.Held)
        {
            HasAnyButtonBeenPressedThisMousePress = true;
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
        var wasMousePreviouslyHoldingButton = button.BoundingBox.Contains(previousMousePosition.X, previousMousePosition.Y) && wasMouseDown;

        if (isMouseCurrentlyHoldingButton)
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
        bool didManuallyPress
    )
    {
        var button = _buttons[index];

        if (index == CurrentlySelectedIndex)
        {
            button.State = didManuallyPress ? UIButton.ButtonState.Held : UIButton.ButtonState.Highlighted;
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
        bool didManuallyPress,
        Vector2 mousePosition, bool isMouseDown,
        Vector2 previousMousePosition, bool wasMouseDown
    )
    {
        if (!isMouseDown)
        {
            HasAnyButtonBeenPressedThisMousePress = false;
        }

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
                TryMarkAnyButtonAsPressed(i);
            }
        }
        else
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                ProcessButtonManual(i, didManuallyPress);
                TryForceSelectedIndex(i);
                TryMarkAnyButtonAsPressed(i);
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
