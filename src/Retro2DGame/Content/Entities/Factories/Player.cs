using Frent;
using Frent.Systems;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using SDL3;
using System.Numerics;

namespace Retro2DGame.Content.Entities.Factories;

internal sealed class PlayerFactory : EntityFactory
{
    public override Entity Create(AssetStorage assets, World world)
    {
        var player = world.Create();

        player.Add(new Dimensions(new Vector2(16, 216) + new Vector2(8, 8), 8));

        player.Add(new Light(8, 8));
        player.Add(
            new LightOscillator
            (
                default,
                16f, 4f, 1f, 0.0f,
                8f, 2f, 0.5f, 0.7f
            )
        );

        player.Tag<EnemyTarget>();

        player.Add(new Sprite(assets.Player.Idle, new Vector2(-8, -8)));
        player.Tag<DrawnIndividually>();

        player.Tag<PlayerCategory>();

        return player;
    }
}