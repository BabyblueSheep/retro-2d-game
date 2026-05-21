using Retro2DGame.Content.Entities;
using Retro2DGame.Content.Entities.Systems;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Retro2DGame.Content.GameStates.GameplayStates;

internal sealed class GameplayState : GameState
{
    public static RectangleF PlayerDimensions => new RectangleF(16, 216, 8, 8);

    private readonly Entity[] _entities;

    private readonly List<EntitySystem> _entitySystems;

    public GameplayState(GameEngine gameEngine) : base(gameEngine)
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
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate(TimeSpan delta)
    {
        for (int i = 0; i < _entities.Length; i++)
        {
            var entity = _entities[i];
            if (!entity.IsActive)
                continue;

        }
    }

    public override void Update(TimeSpan delta)
    {
        for (int i = 0; i < _entities.Length; i++)
        {
            var entity = _entities[i];
            if (!entity.IsActive)
                continue;

            entity.UpdatePreviousDimensions();
        }
    }

    public override void Render(double progress)
    {
        GameEngine.Bitmap.Blit
        (
            GameEngine.AssetStorage.Player.Idle,
            (int)PlayerDimensions.X, (int)PlayerDimensions.Y
        );
    }

    protected override void Dispose(bool disposing)
    {
        
    }
}
