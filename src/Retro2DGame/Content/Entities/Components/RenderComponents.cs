using Frent;
using Frent.Core;
using Frent.Systems;
using Retro2DGame.Content.Entities.Factories;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.NetExtensions;
using System.Numerics;

namespace Retro2DGame.Content.Entities.Components;

internal struct DrawnIndividually;

internal record struct IndependentSpriteOffset(Vector2 Value)
{
    public static void Reset(Level level)
    {
        var spriteEntities = level.World
            .CreateQuery()
            .With<IndependentSpriteOffset>()
            .Build();

        spriteEntities.Delegate((ref IndependentSpriteOffset offset) =>
        {
            offset.Value = Vector2.Zero;
        });
    }
}

internal struct Sprite(PaletteIndexBitmap sprite, Vector2 offset)
{
    public PaletteIndexBitmap Value { get; set; } = sprite;

    public Vector2 Offset { get; set; } = offset;

    public static void RenderSprites(PaletteIndexBitmap destination, Level level, double progress)
    {
        var spriteEntities = level.World
            .CreateQuery()
            .With<Dimensions>()
            .With<Sprite>()
            .Untagged<DrawnIndividually>()
            .Build();

        foreach ((Entity entity, Ref<Dimensions> dimensions, Ref<Sprite> sprite) in spriteEntities.EnumerateWithEntities<Dimensions, Sprite>())
        {
            RenderSprite(destination, entity, progress);
        }
    }

    public static void RenderSprite(PaletteIndexBitmap destination, Entity entity, double progress)
    {
        var dimensions = entity.Get<Dimensions>();
        var sprite = entity.Get<Sprite>();

        var extraOffset = Vector2.Zero;
        if (entity.Has<IndependentSpriteOffset>())
        {
            extraOffset = entity.Get<IndependentSpriteOffset>().Value;
        }

        var position = Vector2.Lerp(dimensions.PreviousPosition, dimensions.Position, (float)progress);

        destination.Blit(sprite.Value, (int)(position.X + sprite.Offset.X + extraOffset.X), (int)(position.Y + sprite.Offset.Y + extraOffset.Y));
    }
}

internal record struct ShakeWhenTakingDamage(TimeSpan Timer, TimeSpan ShakeDuration)
{
    public static void Update(Level level, TimeSpan delta)
    {
        var entitiesThatShouldShake = level.World
            .CreateQuery()
            .With<IndependentSpriteOffset>()
            .With<ShakeWhenTakingDamage>()
            .Build();

        entitiesThatShouldShake.Delegate((ref IndependentSpriteOffset offset, ref ShakeWhenTakingDamage shakeWhenTakingDamage) =>
        {
            shakeWhenTakingDamage.Timer -= delta;
            if (shakeWhenTakingDamage.Timer <= TimeSpan.Zero)
            {
                shakeWhenTakingDamage.Timer = TimeSpan.Zero;
                return;
            }

            float.InverseLerp(0.5f, 0f, 1f);

            var shakeStrength = float.InverseLerp((float)shakeWhenTakingDamage.Timer.TotalSeconds, 0, (float)shakeWhenTakingDamage.ShakeDuration.TotalSeconds);

            offset.Value = new Vector2(level.Random.RandomFloat(-1, 1) * shakeStrength, level.Random.RandomFloat(-1, 1) * shakeStrength);
        });
    }
}

internal record struct ShakeWhenInLight
{
    public static void Update(Level level, Entity lantern)
    {
        var entitiesThatShakeInLight = level.World
            .CreateQuery()
            .With<Dimensions>()
            .With<IndependentSpriteOffset>()
            .Tagged<ShakeWhenInLight>()
            .Build();

        var lanternPosition = lantern.Get<Dimensions>().Position;

        entitiesThatShakeInLight.Delegate((ref Dimensions dimensions, ref IndependentSpriteOffset offset) =>
        {
            var radius = LanternSystems.GetLanternShineLight(lantern);
            if (Vector2.Distance(lanternPosition, dimensions.Position) < radius && lantern.Get<IsFocusing>().Value)
            {
                offset.Value += new Vector2(level.Random.RandomFloat(-1, 1), level.Random.RandomFloat(-1, 1));
            }
        });
    }
}