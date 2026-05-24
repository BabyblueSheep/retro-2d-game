using Frent;
using Frent.Core;
using Frent.Systems;
using Retro2DGame.Content.Entities.Factories;
using SDL3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Entities.Components;

internal record struct Health(int Value);

internal record struct ImmunityFrames(TimeSpan Value);

internal record struct TakesDamageWhenPunched(int AmountReduced)
{
    public static void Update(World world, Entity lantern)
    {
        var lanternPosition = lantern.Get<Dimensions>().Position;

        var entitiesThatTakeDamage = world
            .CreateQuery()
            .With<Dimensions>()
            .With<Health>()
            .With<TakesDamageWhenPunched>()
            .Build();

        entitiesThatTakeDamage.Delegate((ref Dimensions dimensions, ref Health health, ref TakesDamageWhenPunched takesDamageWhenPunched) =>
        {
            if (Vector2.Distance(lanternPosition, dimensions.Position) < dimensions.Radius && lantern.Get<IsPunching>().Value && !lantern.Get<IsPunching>().HasPunchedThisUpdate)
            {
                health.Value -= 1;
                lantern.Get<IsPunching>().HasPunchedThisUpdate = true;
            }
        });
    }
}

internal record struct TakesDamageWhenInShine(TimeSpan Timer, TimeSpan DamageInterval, int AmountReducedWhenReachingInterval)
{
    public static void Update(World world)
    {

    }
}

internal struct DiesWhenHealthReachesZero
{
    public static void Update(World world)
    {
        var entitiesThatShouldDie = world
            .CreateQuery()
            .With<Health>()
            .Tagged<DiesWhenHealthReachesZero>()
            .Build();

        foreach ((Entity entity, Ref<Health> health) in entitiesThatShouldDie.EnumerateWithEntities<Health>())
        {
            if (health.Value.Value <= 0)
            {
                entity.Delete();
            }
        }

    }
}