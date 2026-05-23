using Frent;
using Frent.Core;
using Frent.Systems;
using Retro2DGame.Content.Entities;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Content.Entities.Factories;
using Retro2DGame.Core;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using System.Numerics;
using System.Reflection.Emit;

namespace Retro2DGame.Content.Levels;

internal sealed class Level
{
    public const int LEVEL_WIDTH = GameEngine.GAME_WIDTH;
    public const int LEVEL_HEIGHT = GameEngine.GAME_HEIGHT;
    public static readonly Vector2 LEVEL_SIZE = new Vector2(LEVEL_WIDTH, LEVEL_HEIGHT);

    private readonly PaletteIndexBitmap _levelBitmap = PaletteIndexBitmap.CreateEmpty(LEVEL_WIDTH, LEVEL_HEIGHT);

    public World World { get; }

    private Entity _playerEntity;
    private Entity _lanternEntity;

    private readonly Dictionary<EntityID, EntityFactory> _factories;

    public ConsistentRandom Random { get; }

    public Level()
    {
        DefaultUniformProvider uniformProvider = new DefaultUniformProvider();

        World = new World(uniformProvider);

        _playerEntity = PlayerFactory.CreatePlayer(World);
        _lanternEntity = LanternFactory.CreateLantern(World, Vector2.Zero);

        LightOscillator.UpdateLightOscillators(World, default);

        _factories = new Dictionary<EntityID, EntityFactory>
        {
            { EntityID.GhostGeneric, new GenericGhostFactory() }
        };

        Random = new ConsistentRandom((ulong)DateTime.Now.Ticks);
    }

    public Vector2 GetGenericSpawnPosition()
    {
        const int SPAWN_MARGIN = 32;
        if (Random.Next() % 2 == 0)
            return new Vector2(Random.RandomFloat(0, LEVEL_WIDTH + SPAWN_MARGIN), -SPAWN_MARGIN);
        return new Vector2(LEVEL_WIDTH + SPAWN_MARGIN, Random.RandomFloat(-SPAWN_MARGIN, LEVEL_HEIGHT));
    }

    public void SpawnEntity(EntityID type)
    {
        var entity = _factories[type].Create(World);

        if (entity.Has<SpawnsAtEdges>())
        {
            entity.Get<Dimensions>().Position = GetGenericSpawnPosition();
        }
    }

    public void FixedUpdateEntities(TimeSpan delta)
    {
        
    }

    public void UpdateEntities(TimeSpan delta)
    {
        
    }

    public void UpdateLantern(Inputs inputs, TimeSpan delta)
    {
        LanternSystems.UpdateLanternPosition(inputs, _lanternEntity, delta);
        LanternSystems.UpdateLanternLight(_lanternEntity);
    }

    public bool IsLanternFocused()
    {
        return _lanternEntity.Get<IsFocusing>().Value;
    }

    public void UpdateLights(TimeSpan delta)
    {
        LightOscillator.UpdateLightOscillators(World, delta);
    }

    public void RenderClear()
    {
        _levelBitmap.Clear();
    }

    public void RenderLevel(AssetStorage assets, double progress)
    {
        _levelBitmap.Blit
        (
            assets.Background.Generic,
            0, 0
        );

        PlayerSystems.RenderPlayer(assets, _levelBitmap, _playerEntity);

        LanternSystems.RenderLantern(assets, _levelBitmap, _lanternEntity);

        /*
        for (int i = 0; i < _entities.Length; i++)
        {
            var entity = _entities[i];
            if (!entity.IsActive)
                continue;

            foreach (var system in _entitySystems)
            {
                if (!system.AppliesToType(entity.Type))
                    continue;

                system.Render(this, entity, assets, _levelBitmap, progress);
            }
        }

        _levelBitmap.Blit
        (
            assets.Player.Lantern,
            (int)LanternPosition.X + (int)LanternDrawOffset.X, (int)LanternPosition.Y + (int)LanternDrawOffset.Y
        );*/
    }

    public void RenderDarknessLights(double progress)
    {
        for (int x = 0; x < _levelBitmap.Width; x++)
        {
            for (int y = 0; y < _levelBitmap.Height; y++)
            {
                _levelBitmap.WriteContext(2, x, y);
            }
        }

        var lights = World
            .CreateQuery()
            .With<Dimensions>()
            .With<Light>()
            .Build();

        for (int x = 0; x < _levelBitmap.Width; x++)
        {
            for (int y = 0; y < _levelBitmap.Height; y++)
            {
                var pixelPosition = new Vector2(x, y);

                lights.Delegate((ref Dimensions dimensions, ref Light light) =>
                {
                    if (Vector2.Distance(pixelPosition, dimensions.Position) < (light.InnerRadius + light.OuterRadius))
                    {
                        _levelBitmap.WriteContext(1, x, y);
                    }
                });
            }
        }

        for (int x = 0; x < _levelBitmap.Width; x++)
        {
            for (int y = 0; y < _levelBitmap.Height; y++)
            {
                var pixelPosition = new Vector2(x, y);

                lights.Delegate((ref Dimensions dimensions, ref Light light) =>
                {
                    if (Vector2.Distance(pixelPosition, dimensions.Position) < (light.InnerRadius))
                    {
                        _levelBitmap.WriteContext(0, x, y);
                    }
                });
            }
        }
    }

    public void RenderPresent(PaletteIndexBitmap destination)
    {
        destination.Blit(_levelBitmap, 0, 0);
    }
}
