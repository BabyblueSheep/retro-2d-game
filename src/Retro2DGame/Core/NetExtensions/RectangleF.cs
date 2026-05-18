using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Retro2DGame.Core.NetExtensions;

internal static partial class NetExtensions
{
    extension(RectangleF rectangle)
    {
        public RectangleF Inflated(float x, float y)
        {
            rectangle.X -= x;
            rectangle.Y -= y;
            rectangle.Width += 2 * x;
            rectangle.Height += 2 * y;

            return rectangle;
        }
    }

}