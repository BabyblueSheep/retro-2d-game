using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Content.Entities;

internal abstract class EntitySystem
{
    public abstract EntityType EntityType { get; }

    public bool AppliesToType(EntityType type)
    {
        return type == EntityType;
    }

    public abstract void OnSpawn(Level level, Entity entity);

    public abstract void FixedUpdate(Level level, Entity entity, TimeSpan delta);
    public abstract void Update(Level level, Entity entity, TimeSpan delta);

    public abstract void Render(Level level, Entity entity, AssetStorage assets, PaletteIndexBitmap destination, double progress);
}
