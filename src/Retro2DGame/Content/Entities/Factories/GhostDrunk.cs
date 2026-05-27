using Frent;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Entities.Factories;

internal sealed class GhostDrunkFactory : EntityFactory
{
    public override Entity Create(AssetStorage assets, Level level)
    {
        var entity = level.World.Create();

        entity.Add(new Dimensions(default, 8, default));
        entity.Add(new RadiusDamageMargin(4));

        entity.Add(new SpawnsAtEdges());

        entity.Add(new MovesToTargets(24 + level.Random.RandomFloat(-4f, 4f)));
        entity.Add(new MovesRandomly(16, 0.2f, level.Random.RandomInt(4)));

        entity.Add(new Health(1));
        entity.Add(new TakesDamageWhenInShine(default, TimeSpan.FromSeconds(0.5), 1));
        entity.Tag<DiesWhenHealthReachesZero>();

        entity.Add(new Sprite(assets.Sprites.Enemies.GhostDrunk, new Vector2(-8, -8)));
        entity.Add(new IndependentSpriteOffset(default));
        entity.Tag<ShakeWhenInLight>();

        entity.Add(new PlaySoundWhenDying(assets.Audio.Hurt3));

        entity.Tag<EnemyCategory>();
        entity.Tag<CanKillPlayer>();

        return entity;
    }
}
