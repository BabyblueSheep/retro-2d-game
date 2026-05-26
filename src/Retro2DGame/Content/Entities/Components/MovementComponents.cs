using Frent;
using Frent.Systems;
using Retro2DGame.Content.Levels;
using Retro2DGame.Core.NetExtensions;
using System.Numerics;

namespace Retro2DGame.Content.Entities.Components;

internal struct EnemyTarget;

internal struct DisableMovement;

internal record struct MovesToTargets(float Speed)
{
    public static void Move(Level level, TimeSpan delta)
    {
        var entitiesThatMoveToPlayer = level.World
            .CreateQuery()
            .With<Dimensions>()
            .With<MovesToTargets>()
            .Untagged<DisableMovement>()
            .Build();

        var players = level.World
            .CreateQuery()
            .With<Dimensions>()
            .Tagged<EnemyTarget>()
            .Build();

        entitiesThatMoveToPlayer.Delegate((ref Dimensions entityDimensions, ref MovesToTargets movesToTargets) =>
        {
            var entityPosition = entityDimensions.Position;
            var closestPositionToMoveTo = Vector2.One * 9999;

            players.Delegate((ref Dimensions targetDimensions) =>
            {
                var targetPosition = targetDimensions.Position;

                if (Vector2.Distance(closestPositionToMoveTo, entityPosition) > Vector2.Distance(targetPosition, entityPosition))
                {
                    closestPositionToMoveTo = targetPosition;
                }
            });


            entityDimensions.Position += (closestPositionToMoveTo - entityDimensions.Position)
                .SafeNormalized(Vector2.Zero) * movesToTargets.Speed * (float)delta.TotalSeconds;
        });
    }
}

internal struct MovesRandomly(float speed, float changeSpeed, int initialState)
{
    public float Speed = speed;
    public float ChangeSpeed = changeSpeed;

    public Vector2 MovementVector = Vector2.Zero;
    public int State = initialState;

    public static void Move(Level level, TimeSpan delta)
    {
        var entitiesThatMoveToPlayer = level.World
            .CreateQuery()
            .With<Dimensions>()
            .With<MovesRandomly>()
            .Untagged<DisableMovement>()
            .Build();

        entitiesThatMoveToPlayer.Delegate((ref Dimensions entityDimensions, ref MovesRandomly movesRandomly) =>
        {
            var entityPosition = entityDimensions.Position;
            
            if (level.Random.RandomInt(60) == 0)
            {
                movesRandomly.State = level.Random.RandomInt(4);
            }

            if (movesRandomly.State == 0)
            {
                movesRandomly.MovementVector.X -= movesRandomly.ChangeSpeed * (float)delta.TotalSeconds;
            }
            else if (movesRandomly.State == 1)
            {
                movesRandomly.MovementVector.X += movesRandomly.ChangeSpeed * (float)delta.TotalSeconds;
            }
            else if (movesRandomly.State == 2)
            {
                movesRandomly.MovementVector.Y -= movesRandomly.ChangeSpeed * (float)delta.TotalSeconds;
            }
            else if (movesRandomly.State == 3)
            {
                movesRandomly.MovementVector.Y += movesRandomly.ChangeSpeed * (float)delta.TotalSeconds;
            }

            movesRandomly.MovementVector = movesRandomly.MovementVector.SafeNormalized(Vector2.Zero);

            entityDimensions.Position += movesRandomly.MovementVector * movesRandomly.Speed * (float)delta.TotalSeconds;
        });
    }
}