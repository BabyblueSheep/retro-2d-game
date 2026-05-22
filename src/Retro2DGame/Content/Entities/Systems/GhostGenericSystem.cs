using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.NetExtensions;
using SDL3;
using System.Drawing;
using System.Numerics;

namespace Retro2DGame.Content.Entities.Systems;

internal sealed class GhostGenericSystem : EntitySystem
{
    public override EntityType EntityType => EntityType.GhostGeneric;

    public override void OnSpawn(Level level, Entity entity)
    {
        var position = level.GetGenericSpawnPosition();
        var dimensions = entity.Dimensions;

        dimensions.X = position.X;
        dimensions.Y = position.Y;

        dimensions.Width = 16;
        dimensions.Height = 16;

        entity.Dimensions = dimensions;
    }

    public override void FixedUpdate(Level level, Entity entity, TimeSpan delta)
    {
        var dimensions = entity.Dimensions;
        var center = dimensions.Location.ToVector2();
        center += dimensions.Size.ToVector2() * 0.5f;

        center += (level.PlayerCenter - center).SafeNormalized(Vector2.Zero) * 32 * (float)delta.TotalSeconds;

        center -= dimensions.Size.ToVector2() * 0.5f;
        dimensions.Location = center.ToPointF();
        entity.Dimensions = dimensions;
    }

    public override void Update(Level level, Entity entity, TimeSpan delta)
    {
        
    }

    public override void Render(Level level, Entity entity, AssetStorage assets, PaletteIndexBitmap destination, double progress)
    {
        destination.Blit
        (
            assets.Enemies.Ghost,
            (int)float.Lerp(entity.PreviousDimensions.X, entity.Dimensions.X, (float)progress),
            (int)float.Lerp(entity.PreviousDimensions.Y, entity.Dimensions.Y, (float)progress)
        );
    }
}
