using Frent;
using Frent.Core;
using Frent.Systems;
using Retro2DGame.Content.Entities.Factories;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core;
using Retro2DGame.Core.Game.Audio;
using Retro2DGame.Core.NetExtensions;
using SDL3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Retro2DGame.Content.Entities.Components;

internal record struct Health(int Value);

internal record struct ImmunityFrames(TimeSpan Value)
{
    public static void Update(Level level, TimeSpan delta)
    {
        var entitiesWithImmunityFrames = level.World
            .CreateQuery()
            .With<ImmunityFrames>()
            .Build();

        entitiesWithImmunityFrames.Delegate((ref ImmunityFrames immunityFrames) =>
        {
            immunityFrames.Value -= delta;
            if (immunityFrames.Value <= TimeSpan.Zero)
            {
                immunityFrames.Value = TimeSpan.Zero;
            }
        });
    }
}

internal record struct RadiusDamageMargin(float Value);

internal record struct TakesDamageWhenPunched(int AmountReduced)
{
    public static void Update(Level level, SoundPlayer soundPlayer, Entity lantern)
    {
        var lanternPosition = lantern.Get<Dimensions>().Position;

        var entitiesThatTakeDamage = level.World
            .CreateQuery()
            .With<Dimensions>()
            .With<Health>()
            .With<TakesDamageWhenPunched>()
            .Build();

        foreach ((Entity entity, Ref<Dimensions> dimensions, Ref<Health> health, Ref<TakesDamageWhenPunched> takesDamageWhenPunched) in entitiesThatTakeDamage.EnumerateWithEntities<Dimensions, Health, TakesDamageWhenPunched>())
        {
            if (entity.Has<ImmunityFrames>())
            {
                if (entity.Get<ImmunityFrames>().Value > TimeSpan.Zero)
                {
                    continue;
                }
            }

            var radius = dimensions.Value.Radius;
            if (entity.Has<RadiusDamageMargin>())
            {
                radius += entity.Get<RadiusDamageMargin>().Value;
            }
            if (Vector2.Distance(lanternPosition, dimensions.Value.Position) < radius && lantern.Get<IsPunching>().Value && !lantern.Get<IsPunching>().HasPunchedThisUpdate)
            {
                health.Value.Value -= takesDamageWhenPunched.Value.AmountReduced;
                OnDamageEffectsSystem.Update(level, soundPlayer, entity);
                lantern.Get<IsPunching>().HasPunchedThisUpdate = true;


                if (entity.Has<PlaySoundWhenPunched>() && health.Value.Value > 0)
                {
                    soundPlayer.PlaySound(entity.Get<PlaySoundWhenPunched>().SoundEffect);
                }
            }
        }
    }
}

internal record struct TakesDamageWhenInShine(TimeSpan Timer, TimeSpan DamageInterval, int AmountReducedWhenReachingInterval)
{
    public static void Update(Level level, SoundPlayer soundPlayer, Entity lantern, TimeSpan delta)
    {
        var lanternPosition = lantern.Get<Dimensions>().Position;

        var entitiesThatTakeDamage = level.World
            .CreateQuery()
            .With<Dimensions>()
            .With<Health>()
            .With<TakesDamageWhenInShine>()
            .Build();

        foreach ((Entity entity, Ref<Dimensions> dimensions, Ref<Health> health, Ref<TakesDamageWhenInShine> takesDamageWhenInShine) in entitiesThatTakeDamage.EnumerateWithEntities<Dimensions, Health, TakesDamageWhenInShine>())
        {
            var radius = LanternSystems.GetLanternShineLight(lantern);
            if (Vector2.Distance(lanternPosition, dimensions.Value.Position) < radius && lantern.Get<IsFocusing>().Value)
            {
                takesDamageWhenInShine.Value.Timer += delta;
                if (takesDamageWhenInShine.Value.Timer >= takesDamageWhenInShine.Value.DamageInterval)
                {
                    takesDamageWhenInShine.Value.Timer -= takesDamageWhenInShine.Value.DamageInterval;

                    health.Value.Value -= takesDamageWhenInShine.Value.AmountReducedWhenReachingInterval;
                    OnDamageEffectsSystem.Update(level, soundPlayer, entity);
                }
            }
            else
            {
                takesDamageWhenInShine.Value.Timer = TimeSpan.Zero;
            }
        }
    }
}

internal class OnDamageEffectsSystem
{
    public static void Update(Level level, SoundPlayer soundPlayer, Entity entityThatTookDamage)
    {
        var health = entityThatTookDamage.Get<Health>();

        if (entityThatTookDamage.Tagged<DiesWhenHealthReachesZero>() && health.Value <= 0)
        {
            level.WorldCommandBuffer.DeleteEntity(entityThatTookDamage);

            if (entityThatTookDamage.Has<PlaySoundWhenDying>())
            {
                soundPlayer.PlaySound(entityThatTookDamage.Get<PlaySoundWhenDying>().SoundEffect);
            }
        }

        if (entityThatTookDamage.Has<TeleportsWhenTakingDamage>())
        {
            TeleportsWhenTakingDamage.Teleport(level, entityThatTookDamage);
        }

        if (entityThatTookDamage.Has<ShakeWhenTakingDamage>())
        {
            entityThatTookDamage.Get<ShakeWhenTakingDamage>().Timer = entityThatTookDamage.Get<ShakeWhenTakingDamage>().ShakeDuration;
        }

        if (entityThatTookDamage.Has<ImmunityFramesOnDamage>())
        {
            ImmunityFramesOnDamage.AddImmunityFrames(level, entityThatTookDamage);
        }
    }
}

internal struct DiesWhenHealthReachesZero;

internal struct TeleportsWhenTakingDamage(float distancePushedBack)
{
    public float DistancePushedBack = distancePushedBack;

    public TimeSpan MoveTimer;
    public Vector2 TargetPosition;

    public static void Teleport(Level level, Entity entityToTeleport)
    {
        var players = level.World
            .CreateQuery()
            .With<Dimensions>()
            .Tagged<EnemyTarget>()
            .Build();

        ref var entityDimensions = ref entityToTeleport.Get<Dimensions>();
        ref var teleportsWhenTakingDamage = ref entityToTeleport.Get<TeleportsWhenTakingDamage>();

        var entityPosition = entityDimensions.Position;
        var closestPositionToMoveTo = Vector2.One * 9999;

        players.Delegate((ref Dimensions targetDimensions) =>
        {
            var targetPosition = targetDimensions.Position;

            if (Vector2.Distance(closestPositionToMoveTo, entityPosition) > Vector2.Distance(targetPosition, entityPosition))
            {
                closestPositionToMoveTo = targetPosition;
            }
        });

        teleportsWhenTakingDamage.MoveTimer = TimeSpan.FromSeconds(0.25);

        var distanceToClosestPosition = Vector2.Distance(closestPositionToMoveTo, entityDimensions.Position) + 2;
        var angle = level.Random.RandomFloat(-float.Pi / 2, 0);
        var targetPosition = closestPositionToMoveTo + new Vector2(float.Cos(angle), float.Sin(angle)) * distanceToClosestPosition;
        teleportsWhenTakingDamage.TargetPosition = targetPosition;

        if (!entityToTeleport.Tagged<DisableMovement>())
            level.WorldCommandBuffer.Tag<DisableMovement>(entityToTeleport);
    }

    public static void Update(Level level, TimeSpan delta)
    {
        var enemiesThatCanTeleport = level.World
            .CreateQuery()
            .With<Dimensions>()
            .With<TeleportsWhenTakingDamage>()
            .Build();

        foreach ((Entity entity, Ref<Dimensions> entityDimensions, Ref<TeleportsWhenTakingDamage> teleportsWhenTakingDamage) in enemiesThatCanTeleport.EnumerateWithEntities<Dimensions, TeleportsWhenTakingDamage>())
        {
            if (teleportsWhenTakingDamage.Value.MoveTimer <= TimeSpan.Zero)
                continue;

            var targetPosition = teleportsWhenTakingDamage.Value.TargetPosition;

            var oldTimer = teleportsWhenTakingDamage.Value.MoveTimer;
            teleportsWhenTakingDamage.Value.MoveTimer -= delta;
            if (oldTimer > TimeSpan.Zero && teleportsWhenTakingDamage.Value.MoveTimer <= TimeSpan.Zero)
            {
                level.WorldCommandBuffer.Detach<DisableMovement>(entity);
                entityDimensions.Value.Position = targetPosition;
            }
            else
            {
                var progress = 1 - float.Pow(0.25f, (float)delta.TotalSeconds * 20);
                entityDimensions.Value.Position = Vector2.Lerp(entityDimensions.Value.Position, targetPosition, progress);
            }
        }
    }
}

internal record struct ImmunityFramesOnDamage(TimeSpan Duration)
{
    public static void AddImmunityFrames(Level level, Entity entity)
    {
        entity.Get<ImmunityFrames>().Value += entity.Get<ImmunityFramesOnDamage>().Duration;
    }
}

internal struct PlaySoundWhenPunched(SoundEffect soundEffect)
{
    public SoundEffect SoundEffect = soundEffect;
}

internal struct PlaySoundWhenDying(SoundEffect soundEffect)
{
    public SoundEffect SoundEffect = soundEffect;
}