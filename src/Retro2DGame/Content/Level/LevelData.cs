using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Content.Level;

internal sealed class LevelData
{
    public (LevelWaveData, LevelBreakData)[] Waves { get; }

    public LevelData()
    {
        Waves = [];
    }
}
