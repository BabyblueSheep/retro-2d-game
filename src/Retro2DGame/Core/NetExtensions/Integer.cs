using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.NetExtensions;

internal static partial class NetExtensions
{
    extension(int _)
    {
        public static int Wrap(int number, int limit)
        {
            while (number < 0)
            {
                number += limit;
            }
            number %= limit;
            return number;
        }
    }
}
