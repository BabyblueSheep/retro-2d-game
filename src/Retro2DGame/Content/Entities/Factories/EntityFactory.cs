using Frent;
using Retro2DGame.Core.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Content.Entities.Factories;

internal abstract class EntityFactory
{
    public abstract Entity Create(AssetStorage assets, World world);
}
