using Frent;
using Frent.Systems;
using Retro2DGame.Content.Levels;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Entities.Components;

internal record struct Dimensions(Vector2 Position, float Radius, Vector2 PreviousPosition)
{
    public static void UpdatePreviousDimensions(Level level)
    {
        var positionEntities = level.World
            .CreateQuery()
            .With<Dimensions>()
            .Build();

        positionEntities.Delegate((ref Dimensions dimensions) =>
        {
            dimensions.PreviousPosition = dimensions.Position;
        });
    }
}

internal struct CanKillPlayer;