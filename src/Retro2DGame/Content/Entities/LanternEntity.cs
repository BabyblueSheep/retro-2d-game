using Frent;
using Frent.Systems;
using Retro2DGame.Content.Entities.Components;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core.Game;
using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.NetExtensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Content.Entities;

internal struct LanternTag;

internal class LanternFactory
{
    public static Entity CreateLantern(World world, Vector2 initialPosition)
    {
        var lantern = world.Create();

        lantern.Add(new Dimensions(initialPosition, 8));

        lantern.Add(new IsFocusing(false));

        lantern.Add(new Light(8, 8));
        lantern.Add(
            new LightOscillator
            (
                default,
                24f, 8f, 1f, 0.3f,
                12f, 4f, 0.5f, 1.6f
            )
        );

        lantern.Tag<LanternTag>();

        return lantern;
    }
}

internal class LanternSystems
{
    const float MANUAL_MOVE_DISTANCE = Level.LEVEL_WIDTH / 2;

    public static void UpdateLanternPosition(Inputs inputs, Entity entity, TimeSpan delta)
    {
        if (inputs.MousePosition != inputs.PreviousMousePosition)
        {
            entity.Get<Dimensions>().Position = Vector2.Lerp(Vector2.Zero, Level.LEVEL_SIZE, Vector2.InverseLerp(inputs.MousePosition, Vector2.Zero, GameEngine.GAME_SIZE));
        }
        else
        {
            /*var moveDistance = Vector2.Zero;

            if (inputs.IsDown(InputButtonType.Left))
                moveDistance.X -= 1;
            if (inputs.IsDown(InputButtonType.Right))
                moveDistance.X += 1;

            if (inputs.IsDown(InputButtonType.Up))
                moveDistance.Y -= 1;
            if (inputs.IsDown(InputButtonType.Down))
                moveDistance.Y += 1;

            LanternPosition += moveDistance * MANUAL_MOVE_DISTANCE * (float)delta.TotalSeconds;*/
        }

        entity.Get<Dimensions>().Position = new Vector2
        (
            float.Clamp(entity.Get<Dimensions>().Position.X, 0, Level.LEVEL_WIDTH),
            float.Clamp(entity.Get<Dimensions>().Position.Y, 0, Level.LEVEL_HEIGHT)
        );

        entity.Get<IsFocusing>().Value = inputs.IsMouseLeftClickDown;
    }

    public static void UpdateLanternLight(Entity entity)
    {
        if (entity.Get<IsFocusing>().Value)
        {
            entity.Get<LightOscillator>().BaseInner = 16;
            entity.Get<LightOscillator>().BaseOuter = 8;

            entity.Get<LightOscillator>().OffsetInner = 4;
            entity.Get<LightOscillator>().OffsetOuter = 2;
        }
        else
        {
            entity.Get<LightOscillator>().BaseInner = 24;
            entity.Get<LightOscillator>().BaseOuter = 12;

            entity.Get<LightOscillator>().OffsetInner = 8;
            entity.Get<LightOscillator>().OffsetOuter = 4;
        }
    }

    public const int LANTERN_DRAW_OFFSET_X = -8;
    public const int LANTERN_DRAW_OFFSET_Y = -8;

    public static void RenderLantern(AssetStorage assets, PaletteIndexBitmap destination, Entity entity)
    {
        var position = entity.Get<Dimensions>().Position;

        destination.Blit
        (
            assets.Player.Lantern,
            (int)position.X + LANTERN_DRAW_OFFSET_X, (int)position.Y + LANTERN_DRAW_OFFSET_Y
        );
    }
}

internal record struct IsFocusing(bool Value);