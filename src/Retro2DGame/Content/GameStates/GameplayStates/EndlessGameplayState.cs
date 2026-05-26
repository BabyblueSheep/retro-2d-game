using Retro2DGame.Content.Entities;
using Retro2DGame.Content.GameStates.MenuStates;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using SDL3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Retro2DGame.Content.GameStates.GameplayStates;

internal class EndlessGameplayState : GameState
{
    private readonly Level _level;

    private TimeSpan _cooldownUntilNewEnemies;
    private int _waveNumber;
    private EntityID[] _enemyTypes;

    private bool _isGameOver;

    public EndlessGameplayState(GameEngine gameEngine) : base(gameEngine)
    {
        _level = new Level();

        _level.SpawnSpecialEntities(GameEngine.AssetStorage);
        _level.UpdateLantern(GameEngine.Inputs, default);

        _enemyTypes =
        [
            EntityID.GhostGeneric, EntityID.GhostGeneric, EntityID.GhostGeneric, EntityID.GhostGeneric, EntityID.GhostGeneric, EntityID.GhostGeneric, EntityID.GhostGeneric,
            EntityID.GhostDrunk, EntityID.GhostDrunk, EntityID.GhostDrunk, EntityID.GhostDrunk,
            EntityID.GhostBrute,
            EntityID.GhostTeleporting
        ];

        for (int i = 0; i < _level.Random.RandomInt(3, 5 + 1); i++)
        {
            _level.SpawnEntity(GameEngine.AssetStorage, _enemyTypes[_level.Random.RandomInt(_enemyTypes.Length)]);
        }
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate(TimeSpan delta)
    {
        if (_isGameOver)
        {
            return;
        }

        _level.FixedUpdateEntities(delta);

        _level.UpdateFixedLantern(GameEngine.Inputs, delta);

        _level.WorldCommandBuffer.Playback();

        if (_level.IsGameOver())
        {
            _isGameOver = true;
            return;
        }

        if (_level.GetAliveEnemyCount() <= 0)
        {
            _cooldownUntilNewEnemies += delta;

            if (_cooldownUntilNewEnemies >= TimeSpan.FromSeconds(1))
            {
                _cooldownUntilNewEnemies = TimeSpan.Zero;

                _waveNumber++;
                for (int i = 0; i < _level.Random.RandomInt(3, 5 + 1) + _waveNumber; i++)
                {
                    _level.SpawnEntity(GameEngine.AssetStorage, _enemyTypes[_level.Random.RandomInt(_enemyTypes.Length)]);
                }
            }
        }
    }

    public override void Update(TimeSpan delta)
    {
        if (GameEngine.Inputs.IsDown(InputButtonType.Pause) && !GameEngine.Inputs.WasDown(InputButtonType.Pause))
        {
            GameEngine.GameStates.Pop();
            GameEngine.GameStates.Push(new MainMenuBackgroundState(GameEngine));
            GameEngine.GameStates.Push(new MainMenuLevelSelectState(GameEngine));
            return;
        }

        if (_isGameOver)
        {
            return;
        }

        _level.UpdateEntities(delta);

        _level.UpdateLantern(GameEngine.Inputs, delta);

        _level.UpdateLights(delta);

        if (_level.IsLanternFocused())
        {
            GameEngine.Palette[16, 0] = Color.White; GameEngine.Palette[16, 1] = Color.White; GameEngine.Palette[16, 2] = Color.White;
            GameEngine.Palette[17, 0] = Color.Lime; GameEngine.Palette[17, 1] = Color.Lime; GameEngine.Palette[17, 2] = Color.Lime;
            GameEngine.Palette[18, 0] = Color.Green; GameEngine.Palette[18, 1] = Color.Green; GameEngine.Palette[18, 2] = Color.Green;
        }
        else
        {
            GameEngine.Palette[16, 0] = Color.White; GameEngine.Palette[16, 1] = Color.White; GameEngine.Palette[16, 2] = Color.White;
            GameEngine.Palette[17, 0] = Color.Orange; GameEngine.Palette[17, 1] = Color.Orange; GameEngine.Palette[17, 2] = Color.Orange;
            GameEngine.Palette[18, 0] = Color.Red; GameEngine.Palette[18, 1] = Color.Red; GameEngine.Palette[18, 2] = Color.Red;
        }
    }

    public override void Render(double progress)
    {
        if (!_isGameOver)
        {
            _level.RenderClear();
            _level.RenderLevel(GameEngine.AssetStorage, progress);
            _level.RenderDarknessLights(progress);
        }
        _level.RenderPresent(GameEngine.Bitmap);

        TextRenderer.BlitText
        (
            GameEngine.AssetStorage, GameEngine.Bitmap,
            8, 8,
            $"Ghosts left: {_level.GetAliveEnemyCount()}"
        );

        TextRenderer.BlitText
        (
            GameEngine.AssetStorage, GameEngine.Bitmap,
            8, 16,
            $"Wave: {_waveNumber + 1}"
        );

        if (_isGameOver)
        {
            TextRenderer.BlitText
        (
            GameEngine.AssetStorage, GameEngine.Bitmap,
            128, 128 - 4,
            "Game Over",
            TextRenderer.TextAlignment.Center
        );
        }
    }

    protected override void Dispose(bool disposing)
    {

    }
}
