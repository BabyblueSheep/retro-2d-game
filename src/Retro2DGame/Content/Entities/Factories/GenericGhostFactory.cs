using Frent;
using Retro2DGame.Content.Entities.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Retro2DGame.Content.Entities.Factories;

internal sealed class GenericGhostFactory : EntityFactory
{
    public override Entity Create(World world)
    {
        var entity = world.Create();

        entity.Add(new Dimensions(default, 8));

        entity.Add(new SpawnsAtEdges());

        return entity;
    }
}
