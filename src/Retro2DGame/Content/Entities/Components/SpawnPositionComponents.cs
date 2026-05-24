using Frent.Components;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Entities.Components;

internal struct SpawnsAtEdges
{
    public static Vector2 GetPosition(ConsistentRandom random)
    {
        const int SPAWN_MARGIN = -16;
        if (random.Next() % 2 == 0)
            return new Vector2(random.RandomFloat(0, Level.LEVEL_WIDTH + SPAWN_MARGIN), -SPAWN_MARGIN);
        return new Vector2(Level.LEVEL_WIDTH + SPAWN_MARGIN, random.RandomFloat(-SPAWN_MARGIN, Level.LEVEL_HEIGHT));
    }
}