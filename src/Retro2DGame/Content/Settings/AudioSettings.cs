using Game.Core.Game.Settings.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Content.Settings;

internal sealed class AudioSettings
{
    [DecimalNumberField(Start = 0.0, End = 1.0)]
    [Position(0)]
    public double MasterVolume
    {
        get;
        set => field = double.Clamp(value, 0.0, 1.0);
    }

    [DecimalNumberField(Start = 0.0, End = 1.0)]
    [Position(1)]
    public double MusicVolume
    {
        get;
        set => field = double.Clamp(value, 0.0, 1.0);
    }

    [DecimalNumberField(Start = 0.0, End = 1.0)]
    [Position(2)]
    public double SoundsVolume
    {
        get;
        set => field = double.Clamp(value, 0.0, 1.0);
    }

    public static int OptionCount => 3;
}

