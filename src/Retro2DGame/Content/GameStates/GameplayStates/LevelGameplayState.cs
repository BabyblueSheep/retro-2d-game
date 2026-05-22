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
        _level.SpawnEntity(EntityType.GhostGeneric);
        _level.SpawnEntity(EntityType.GhostGeneric);
        _level.SpawnEntity(EntityType.GhostGeneric);
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
        _level.UpdateLanternPosition(GameEngine.Inputs, delta);
        _level.UpdateEntities(delta);
    }

    public override void Render(double progress)
    {
        _level.RenderEntities(GameEngine.AssetStorage, GameEngine.Bitmap, progress);
    }

    protected override void Dispose(bool disposing)
    {
        
    }
}
