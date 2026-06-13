using Frent;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Entities.Factories;

internal class GhostBruteFactory : EntityFactory
{
    public override Entity Create(AssetStorage assets, Level level)
    {
        var entity = level.World.Create();

        entity.Add(new Dimensions(default, 16, default));
        entity.Add(new RadiusDamageMargin(4));

        entity.Add(new SpawnsAtEdges());

        entity.Add(new MovesToTargets(16));

        entity.Add(new Health(6));
        entity.Add(new ImmunityFrames(default));
        entity.Add(new TakesDamageWhenPunched(1));
        entity.Tag<DiesWhenHealthReachesZero>();

        entity.Add(new Sprite(assets.Sprites.Enemies.GhostBrute, new Vector2(-16, -16)));
        entity.Add(new IndependentSpriteOffset(default));
        entity.Add(new ShakeWhenTakingDamage(default, TimeSpan.FromSeconds(1)));

        entity.Tag<EnemyCategory>();
        entity.Tag<CanKillPlayer>();

        return entity;
    }
}