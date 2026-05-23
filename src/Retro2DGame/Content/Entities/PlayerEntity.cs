using Frent;
using Frent.Systems;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using SDL3;
using System.Numerics;

namespace Retro2DGame.Content.Entities;

internal struct PlayerTag;

internal class PlayerFactory
{
    public static Entity CreatePlayer(World world)
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

        player.Tag<PlayerTag>();

        return player;
    }
}

internal class PlayerSystems
{
    public const int PLAYER_DRAW_OFFSET_X = -8;
    public const int PLAYER_DRAW_OFFSET_Y = -8;

    public static void RenderPlayer(AssetStorage assets, PaletteIndexBitmap destination, Entity entity)
    {
        var position = entity.Get<Dimensions>().Position;

        destination.Blit
        (
            assets.Player.Idle,
            (int)position.X + PLAYER_DRAW_OFFSET_X, (int)position.Y + PLAYER_DRAW_OFFSET_Y
        );
    }
}