using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game;

internal sealed class Settings
{
    private int _volume;
    public int Volume
    {
        get => _volume;
        set
        {
            _volume = int.Clamp(value, 0, 100);
        }
    }


}
