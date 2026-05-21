using Retro2DGame.Content.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Content.Level;

internal sealed class LevelWaveData
{
    public (EntityType, float)[] AvailableEntityTypesCosts { get; private set; }
    public float TotalCost { get; private set; }

    public LevelWaveData()
    {
        AvailableEntityTypesCosts = [];
    }
}
