using Frent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Content.Entities.Factories;

internal abstract class EntityFactory
{
    public abstract Entity Create(World world);
}
