using Retro2DGame.Content.Entities;
using Retro2DGame.Content.Entities.Systems;
using Retro2DGame.Core;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.NetExtensions;
using SDL3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Retro2DGame.Content.Levels;

internal sealed class Level
{
    public const int LEVEL_WIDTH = GameEngine.GAME_WIDTH;
    public const int LEVEL_HEIGHT = GameEngine.GAME_HEIGHT;
    public readonly Vector2 LEVEL_SIZE = new Vector2(LEVEL_WIDTH, LEVEL_HEIGHT);

    public Vector2 PlayerCenter { get; } = new Vector2(16, 216) + new Vector2(8, 8);
    public Vector2 PlayerDrawOffset { get; } = new Vector2(-8, -8);

    public Vector2 LanternPosition { get; private set; }
    public Vector2 LanternDrawOffset { get; } = new Vector2(-8, -8);

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

        Random = new ConsistentRandom((ulong)DateTime.Now.Ticks);
    }

    public Vector2 GetGenericSpawnPosition()
    {
        const int SPAWN_MARGIN = 32;
        if (Random.Next() % 2 == 0)
            return new Vector2(Random.RandomFloat(0, LEVEL_WIDTH + SPAWN_MARGIN), -SPAWN_MARGIN);
        return new Vector2(LEVEL_WIDTH + SPAWN_MARGIN, Random.RandomFloat(-SPAWN_MARGIN, LEVEL_HEIGHT));
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

    public void FixedUpdateEntities(TimeSpan delta)
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

    public void UpdateEntities(TimeSpan delta)
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

    public void UpdateLanternPosition(Inputs inputs, TimeSpan delta)
    {
        const float MANUAL_MOVE_DISTANCE = LEVEL_WIDTH / 2;

        if (inputs.MousePosition != inputs.PreviousMousePosition)
        {
            LanternPosition = Vector2.Lerp(Vector2.Zero, LEVEL_SIZE, Vector2.InverseLerp(inputs.MousePosition, Vector2.Zero, GameEngine.GAME_SIZE));
        }
        else
        {
            var moveDistance = Vector2.Zero;

            if (inputs.IsDown(InputButtonType.Left))
                moveDistance.X -= 1;
            if (inputs.IsDown(InputButtonType.Right))
                moveDistance.X += 1;

            if (inputs.IsDown(InputButtonType.Up))
                moveDistance.Y -= 1;
            if (inputs.IsDown(InputButtonType.Down))
                moveDistance.Y += 1;

            LanternPosition += moveDistance * MANUAL_MOVE_DISTANCE * (float)delta.TotalSeconds;
        }

        LanternPosition = new Vector2
        (
            float.Clamp(LanternPosition.X, 0, LEVEL_WIDTH),
            float.Clamp(LanternPosition.Y, 0, LEVEL_HEIGHT)
        );

        SDL.LogInfo(SDL.LogCategory.Application, $"{LanternPosition}");
    }

    public void RenderEntities(AssetStorage assets, PaletteIndexBitmap destination, double progress)
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

        destination.Blit
        (
            assets.Player.Lantern,
            (int)LanternPosition.X + (int)LanternDrawOffset.X, (int)LanternPosition.Y + (int)LanternDrawOffset.Y
        );
    }
}
