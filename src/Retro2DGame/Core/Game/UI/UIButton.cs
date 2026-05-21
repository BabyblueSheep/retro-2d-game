using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.NetExtensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Core.Game.UI;

internal abstract class UIButton
{
    internal enum ButtonState
    {
        Idle,
        Highlighted,
        Held,
    }

    private ButtonState _state;
    public ButtonState PreviousState { get; private set; }
    public ButtonState State
    {
        get { return _state; }
        set
        {
            var didStateChange = _state != value;
            _state = value;
            if (didStateChange)
            {
                OnStateChange(this);
            }
        }
    }

    public RectangleF Dimensions { get; set; }
    public Vector2 Margin { get; set; }

    public RectangleF BoundingBox => Dimensions.Inflated(Margin.X, Margin.Y);

    public Action<UIButton> OnStateChange { get; }

    public UIButton(Action<UIButton> onStateChange)
    {
        OnStateChange = onStateChange;
    }

    public void PropagateState()
    {
        PreviousState = _state;
    }

    public abstract void Render(AssetStorage assets, PaletteIndexBitmap destination);
}

internal sealed class UITextButton : UIButton
{
    private string _text;

    public string Text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            Dimensions = new RectangleF(Dimensions.X, Dimensions.Y, TextRenderer.GetTextWidth(_text), TextRenderer.GetTextHeight());
        }
    }

    public TextRenderer.TextAlignment TextAlignment { get; set; } = TextRenderer.TextAlignment.Left;

    public UITextButton(Action<UIButton> onStateChange) : base(onStateChange)
    {
        _text = "";
    }

    public override void Render(AssetStorage assets, PaletteIndexBitmap destination)
    {
        var textToRender = Text;
        if (State == ButtonState.Highlighted)
        {
            textToRender = string.Concat("*", Text, "*");
        }
        if (State == ButtonState.Held)
        {
            textToRender = string.Concat("#", Text, "#");
        }

        var positionToRenderX = (int)Dimensions.Left;
        if (TextAlignment == TextRenderer.TextAlignment.Right)
        {
            positionToRenderX = (int)Dimensions.Right;
        }
        if (TextAlignment == TextRenderer.TextAlignment.Center)
        {
            positionToRenderX = ((int)Dimensions.Left + (int)Dimensions.Right) / 2;
        }

        TextRenderer.BlitText(assets, destination, positionToRenderX, (int)Dimensions.Y, textToRender, TextAlignment);
    }
}