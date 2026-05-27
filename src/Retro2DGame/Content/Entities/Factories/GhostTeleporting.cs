using Frent;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Entities.Factories;

internal sealed class GhostTeleportingFactory : EntityFactory
{
    public override Entity Create(AssetStorage assets, Level level)
    {
        var entity = level.World.Create();

        entity.Add(new Dimensions(default, 8, default));
        entity.Add(new RadiusDamageMargin(4));

        entity.Add(new SpawnsAtEdges());

        entity.Add(new MovesToTargets(48 + level.Random.RandomFloat(-12f, 4f)));

        entity.Add(new Health(3));
        entity.Add(new ImmunityFrames(default));
        entity.Add(new TakesDamageWhenPunched(1));
        entity.Tag<DiesWhenHealthReachesZero>();
        entity.Add(new ImmunityFramesOnDamage(TimeSpan.FromSeconds(0.25)));
        entity.Add(new TeleportsWhenTakingDamage(10));

        entity.Add(new Sprite(assets.Sprites.Enemies.GhostTeleporting, new Vector2(-8, -8)));
        entity.Add(new IndependentSpriteOffset(default));
        entity.Add(new ShakeWhenTakingDamage(default, TimeSpan.FromSeconds(1)));

        entity.Add(new PlaySoundWhenPunched(assets.Audio.Hurt1));
        entity.Add(new PlaySoundWhenDying(assets.Audio.Hurt3));

        entity.Tag<EnemyCategory>();
        entity.Tag<CanKillPlayer>();

        return entity;
    }
}