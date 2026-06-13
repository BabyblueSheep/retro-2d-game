using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Audio;

internal struct AudioPlayParameters
{
    public static AudioPlayParameters Default = new AudioPlayParameters() with { Volume = 1, Pitch = 1 };

    public float Volume { get; set; }
    public float Pitch { get; set; }
}
