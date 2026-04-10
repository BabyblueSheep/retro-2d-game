using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Content.Settings;

internal sealed class Settings
{
    public AudioSettings AudioOptions { get; }

    public Settings()
    {
        AudioOptions = new AudioSettings();
    }
}
