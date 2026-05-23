using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.NetExtensions;
using SDL3;
using System.Drawing;
using System.Numerics;

namespace Retro2DGame.Content.Entities.Systems;

/*
internal sealed class GhostGenericSystem : EntitySystem
{
    public override EntityType EntityType => EntityType.GhostGeneric;

    public override void OnSpawn(Level level, Entity entity)
    {
        entity.CenterPosition = level.GetGenericSpawnPosition();

        entity.Radius = 8;
    }

    public override void FixedUpdate(Level level, Entity entity, TimeSpan delta)
    {
        entity.CenterPosition += (level.PlayerCenter - entity.CenterPosition).SafeNormalized(Vector2.Zero) * 32 * (float)delta.TotalSeconds;
    }

    public override void Update(Level level, Entity entity, TimeSpan delta)
    {
        entity.ArbitraryBooleanOne = false;
    }

    public override void FixedUpdateInShine(Level level, Entity entity, TimeSpan delta)
    {
        
    }

    public override void FixedUpdateInFocusedShine(Level level, Entity entity, TimeSpan delta)
    {
        entity.ArbitraryBooleanOne = true;
    }

    public override void Render(Level level, Entity entity, AssetStorage assets, PaletteIndexBitmap destination, double progress)
    {
        var offsetPosition = Vector2.Zero;
        if (entity.ArbitraryBooleanOne)
            offsetPosition = new Vector2(level.Random.RandomFloat(-1f, 1f), level.Random.RandomFloat(-1f, 1f));

        destination.Blit
        (
            assets.Enemies.Ghost,
            (int)(entity.CenterPosition.X + offsetPosition.X) - 8,
            (int)(entity.CenterPosition.Y + offsetPosition.Y) - 8
        );
    }
}
*/