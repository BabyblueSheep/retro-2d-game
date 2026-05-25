using Frent;
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

    private (Vector2, float, float)[] _cachedLights;

    private readonly Dictionary<EntityID, EntityFactory> _factories;

    public ConsistentRandom Random { get; }

    public Level()
    {
        World = new World();

        _cachedLights = new (Vector2, float, float)[2];

        _factories = new Dictionary<EntityID, EntityFactory>
        {
            { EntityID.Lantern, new LanternFactory() },
            { EntityID.Player, new PlayerFactory() },
            { EntityID.GhostGeneric, new GenericGhostFactory() }
        };

        Random = new ConsistentRandom((ulong)DateTime.Now.Ticks);
    }

    public void SpawnSpecialEntities(AssetStorage assets)
    {
        _playerEntity = _factories[EntityID.Player].Create(assets, World);
        _lanternEntity = _factories[EntityID.Lantern].Create(assets, World);
    }

    public void SpawnEntity(AssetStorage assets, EntityID type)
    {
        var entity = _factories[type].Create(assets, World);

        if (entity.Has<SpawnsAtEdges>())
        {
            entity.Get<Dimensions>().Position = SpawnsAtEdges.GetPosition(Random);
        }
    }

    public void FixedUpdateEntities(TimeSpan delta)
    {
        MovesToTargets.Move(World, delta);

        DiesWhenHealthReachesZero.Update(World);
    }

    public void UpdateEntities(TimeSpan delta)
    {
        
    }

    public int GetAliveEnemyCount()
    {
        var enemies = World
            .CreateQuery()
            .Tagged<EnemyCategory>()
            .Build();

        var count = 0;
        foreach (Entity entity in enemies.EnumerateWithEntities())
        {
            count++;
        }

        return count;
    }

    public void UpdateLantern(Inputs inputs, TimeSpan delta)
    {
        LanternSystems.UpdateLanternPosition(inputs, _lanternEntity, delta);
        LanternSystems.UpdateLanternLight(_lanternEntity);

        TakesDamageWhenPunched.Update(World, _lanternEntity);
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

        Sprite.RenderSprite(_levelBitmap, _playerEntity, progress);

        Sprite.RenderSprites(_levelBitmap, World, progress);

        Sprite.RenderSprite(_levelBitmap, _lanternEntity, progress);
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

        var lightCounter = 0;
        lights.Delegate((ref Dimensions dimensions, ref Light light) =>
        {
            lightCounter++;
        });

        if (lightCounter != _cachedLights.Length)
        {
            Array.Resize(ref _cachedLights, lightCounter);
        }

        lightCounter = 0;
        lights.Delegate((ref Dimensions dimensions, ref Light light) =>
        {
            _cachedLights[lightCounter] = (dimensions.Position, light.InnerRadius, light.OuterRadius);
            lightCounter++;
        });

        for (int x = 0; x < _levelBitmap.Width; x++)
        {
            for (int y = 0; y < _levelBitmap.Height; y++)
            {
                var pixelPosition = new Vector2(x, y);

                foreach (var light in _cachedLights)
                {
                    if (Vector2.Distance(pixelPosition, light.Item1) < (light.Item2 + light.Item3))
                    {
                        _levelBitmap.WriteContext(1, x, y);
                    }
                }
            }
        }

        for (int x = 0; x < _levelBitmap.Width; x++)
        {
            for (int y = 0; y < _levelBitmap.Height; y++)
            {
                var pixelPosition = new Vector2(x, y);

                foreach (var light in _cachedLights)
                {
                    if (Vector2.Distance(pixelPosition, light.Item1) < light.Item2)
                    {
                        _levelBitmap.WriteContext(0, x, y);
                    }
                }
            }
        }
    }

    public void RenderPresent(PaletteIndexBitmap destination)
    {
        destination.Blit(_levelBitmap, 0, 0);
    }
}
