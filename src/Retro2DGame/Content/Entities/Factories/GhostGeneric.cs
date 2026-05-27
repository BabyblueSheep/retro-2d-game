using Frent;
using Frent.Systems;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using System.Numerics;

namespace Retro2DGame.Content.Entities.Factories;

internal sealed class GhostGenericFactory : EntityFactory
{
    public override Entity Create(AssetStorage assets, Level level)
    {
        var entity = level.World.Create();

        entity.Add(new Dimensions(default, 8, default));
        entity.Add(new RadiusDamageMargin(4));

        entity.Add(new SpawnsAtEdges());

        entity.Add(new MovesToTargets(32 + level.Random.RandomFloat(-8f, 8f)));

        entity.Add(new Health(1));
        entity.Add(new TakesDamageWhenPunched(1));
        entity.Tag<DiesWhenHealthReachesZero>();

        entity.Add(new Sprite(assets.Sprites.Enemies.GhostGeneric, new Vector2(-8, -8)));
        entity.Add(new IndependentSpriteOffset(default));

        entity.Add(new PlaySoundWhenDying(assets.Audio.Hurt3));

        entity.Tag<EnemyCategory>();
        entity.Tag<CanKillPlayer>();

        return entity;
    }
}
