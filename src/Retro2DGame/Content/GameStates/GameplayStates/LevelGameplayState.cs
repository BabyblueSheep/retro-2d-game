using Retro2DGame.Content.Entities;
using Retro2DGame.Content.Entities.Systems;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Retro2DGame.Content.GameStates.GameplayStates;

internal sealed class LevelGameplayState : GameState
{
    private readonly Level _level;

    public LevelGameplayState(GameEngine gameEngine) : base(gameEngine)
    {
        _level = new Level();
    }

    public override void Enter()
    {
        _level.SpawnEntity(EntityID.GhostGeneric);
        _level.SpawnEntity(EntityID.GhostGeneric);
        _level.SpawnEntity(EntityID.GhostGeneric);
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate(TimeSpan delta)
    {
        _level.FixedUpdateEntities(delta);
    }

    public override void Update(TimeSpan delta)
    {
        _level.UpdateLantern(GameEngine.Inputs, delta);
        _level.UpdateEntities(delta);
        _level.UpdateLights(delta);
    }

    public override void Render(double progress)
    {
        _level.RenderClear();
        _level.RenderLevel(GameEngine.AssetStorage, progress);
        _level.RenderDarknessLights(progress);
        _level.RenderPresent(GameEngine.Bitmap);

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

    protected override void Dispose(bool disposing)
    {
        
    }
}
