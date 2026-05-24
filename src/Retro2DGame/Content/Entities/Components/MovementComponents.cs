using Frent;
using Frent.Systems;
using Retro2DGame.Core.NetExtensions;
using System.Numerics;

namespace Retro2DGame.Content.Entities.Components;

internal struct EnemyTarget;

internal record struct MovesToTargets(float Speed)
{
    public static void Move(World world, TimeSpan delta)
    {
        var entitiesThatMoveToPlayer = world
            .CreateQuery()
            .With<Dimensions>()
            .With<MovesToTargets>()
            .Build();

        var players = world
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