using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Retro2DGame.Content.Entities;

internal sealed class Entity
{
    public EntityType Type { get; private set; }

    public RectangleF PreviousDimensions { get; private set; }
    public RectangleF Dimensions { get; set; }

    public bool IsActive { get; set; }

    public void UpdatePreviousDimensions()
    {
        PreviousDimensions = Dimensions;
    }
}
