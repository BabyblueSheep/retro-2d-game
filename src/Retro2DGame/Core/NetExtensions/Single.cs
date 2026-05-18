using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.NetExtensions;

internal static partial class NetExtensions
{
    extension (float _)
    {
        public static float InverseLerp(float x, float a, float b)
        {
            return (x - a) / (b - a);
        }
    }
}
