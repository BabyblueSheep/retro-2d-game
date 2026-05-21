using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Content.Entities;

internal abstract class EntitySystem
{
    public abstract EntityType EntityType { get; }

    //public abstract void OnSpawn(Entity entity);
}
