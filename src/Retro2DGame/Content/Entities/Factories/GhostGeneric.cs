using Frent;
using Frent.Systems;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Core;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using System.Numerics;

namespace Retro2DGame.Content.Entities.Factories;

internal sealed class GenericGhostFactory : EntityFactory
{
    public override Entity Create(AssetStorage assets, World world)
    {
        var entity = world.Create();

        entity.Add(new Dimensions(default, 12));

        entity.Add(new SpawnsAtEdges());

        entity.Add(new MovesToTargets(32));

        entity.Add(new Health(1));
        entity.Add(new TakesDamageWhenPunched(1));
        entity.Tag<DiesWhenHealthReachesZero>();

        entity.Add(new Sprite(assets.Enemies.Ghost, new Vector2(-8, -8)));
        entity.Add(new IndependentSpriteOffset(default));

        entity.Tag<EnemyCategory>();

        return entity;
    }
}
