using System;
using System.Collections.Generic;
using System.Drawing;
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
    private UIButtonState _previousState;
    public UIButtonState State { get; set; }
    public RectangleF BoundingBox { get; set; }

    public Action UpdateFunction { get; }
    public Action RenderFunction { get; }

    public UIButton(Action updateFunction, Action renderFunction)
    {
        UpdateFunction = updateFunction;
        RenderFunction = renderFunction;
    }

    public void ProcessMouse()
    {

    }
}
