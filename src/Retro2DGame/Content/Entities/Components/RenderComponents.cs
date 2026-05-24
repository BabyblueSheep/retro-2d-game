using Frent;
using Frent.Systems;
using Retro2DGame.Core.Game.Rendering;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Entities.Components;

internal struct DrawnIndividually;

internal record struct IndependentSpriteOffset(Vector2 Value);

internal struct Sprite(PaletteIndexBitmap sprite, Vector2 offset)
{
    public PaletteIndexBitmap Value { get; set; } = sprite;

    public Vector2 Offset { get; set; } = offset;

    public static void RenderSprites(PaletteIndexBitmap destination, World world, double progress)
    {
        var spriteEntities = world
            .CreateQuery()
            .With<Dimensions>()
            .With<Sprite>()
            .Untagged<DrawnIndividually>()
            .Build();

        spriteEntities.Delegate((ref Dimensions dimensions, ref Sprite sprite) =>
        {
            destination.Blit(sprite.Value, (int)(dimensions.Position.X + sprite.Offset.X), (int)(dimensions.Position.Y + sprite.Offset.Y));
        });
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

        destination.Blit(sprite.Value, (int)(dimensions.Position.X + sprite.Offset.X + extraOffset.X), (int)(dimensions.Position.Y + sprite.Offset.Y + extraOffset.Y));
    }
}
