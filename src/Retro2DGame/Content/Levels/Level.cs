using Retro2DGame.Content.Entities;
using Retro2DGame.Content.Entities.Systems;
using Retro2DGame.Core;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using SDL3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Levels;

internal sealed class Level
{
    public const int LEVEL_WIDTH = GameEngine.GAME_WIDTH;
    public const int LEVEL_HEIGHT = GameEngine.GAME_HEIGHT;
    public readonly Vector2 LEVEL_SIZE = new Vector2(LEVEL_WIDTH, LEVEL_HEIGHT);

    public Vector2 PlayerCenter { get; } = new Vector2(16, 216) + new Vector2(8, 8);
    public Vector2 PlayerDrawOffset { get; } = new Vector2(-8, -8);

    private readonly Entity[] _entities;

    private readonly List<EntitySystem> _entitySystems;

    public ConsistentRandom Random { get; }

    public bool HasAnyAliveEnemies { get; private set; }

    public Level()
    {
        _entities = new Entity[200];
        for (int i = 0; i < _entities.Length; i++)
        {
            _entities[i] = new Entity();
        }

        _entitySystems =
        [
            new GhostGenericSystem()
        ];

        Random = new ConsistentRandom();
    }

    public Vector2 GetGenericSpawnPosition()
    {
        if (Random.Next() % 2 == 0)
            return new Vector2(Random.RandomFloat(0, LEVEL_WIDTH + 32), -32);
        return new Vector2(LEVEL_WIDTH + 32, Random.RandomFloat(-32, LEVEL_HEIGHT));
    }

    public void SpawnEntity(EntityType type)
    {
        for (int i = 0; i < _entities.Length; i++)
        {
            var entity = _entities[i];
            if (entity.IsActive)
                continue;

            entity.IsActive = true;

            entity.Type = type;
            foreach (var system in _entitySystems)
            {
                if (!system.AppliesToType(entity.Type))
                    continue;

                system.OnSpawn(this, entity);
            }

            break;
        }
    }

    public void FixedUpdate(TimeSpan delta)
    {
        for (int i = 0; i < _entities.Length; i++)
        {
            var entity = _entities[i];
            if (!entity.IsActive)
                continue;

            foreach (var system in _entitySystems)
            {
                if (!system.AppliesToType(entity.Type))
                    continue;

                system.FixedUpdate(this, entity, delta);
            }
        }
    }

    public void Update(TimeSpan delta)
    {
        for (int i = 0; i < _entities.Length; i++)
        {
            var entity = _entities[i];
            if (!entity.IsActive)
                continue;

            entity.UpdatePreviousDimensions();

            foreach (var system in _entitySystems)
            {
                if (!system.AppliesToType(entity.Type))
                    continue;

                system.Update(this, entity, delta);
            }
        }
    }

    public void Render(AssetStorage assets, PaletteIndexBitmap destination, double progress)
    {
        destination.Blit
        (
            assets.Player.Idle,
            (int)PlayerCenter.X + (int)PlayerDrawOffset.X, (int)PlayerCenter.Y + (int)PlayerDrawOffset.Y
        );

        for (int i = 0; i < _entities.Length; i++)
        {
            var entity = _entities[i];
            if (!entity.IsActive)
                continue;

            foreach (var system in _entitySystems)
            {
                if (!system.AppliesToType(entity.Type))
                    continue;

                system.Render(this, entity, assets, destination, progress);
            }
        }
    }
}
