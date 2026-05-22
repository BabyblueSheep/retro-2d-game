using Retro2DGame.Core.NetExtensions;
using SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Core.NetExtensions;

internal static partial class NetExtensions
{
    extension (Vector2 vector)
    {
        public static Vector2 InverseLerp(Vector2 x, Vector2 a, Vector2 b)
        {
            return (x - a) / (b - a);
        }

        public static Vector2 RemapLetterboxed(Vector2 vectorToRemap, Vector2 originalSize, Vector2 targetSize)
        {
            var originalAspectRatio = originalSize.X / originalSize.Y;
            var targetAspectRatio = targetSize.X / targetSize.Y;

            if (targetAspectRatio > originalAspectRatio)
            {
                var scale = originalSize.X / targetSize.X;
                Vector2 letterboxSize = new Vector2(originalSize.X, float.Floor(targetSize.Y * scale));
                Vector2 letterboxOrigin = new Vector2(0, (originalSize.Y - letterboxSize.Y) / 2);

                Vector2 relativeVector = Vector2.InverseLerp(vectorToRemap, letterboxOrigin, letterboxOrigin + letterboxSize);
                relativeVector.Y = float.Clamp(relativeVector.Y, 0, 1);

                return relativeVector * targetSize;
            }
            else
            {
                var scale = originalSize.Y / targetSize.Y;
                Vector2 letterboxSize = new Vector2(float.Floor(targetSize.X * scale), originalSize.Y);
                Vector2 letterboxOrigin = new Vector2((originalSize.X - letterboxSize.X) / 2, 0);

                Vector2 relativeVector = Vector2.InverseLerp(vectorToRemap, letterboxOrigin, letterboxOrigin + letterboxSize);
                relativeVector.X = float.Clamp(relativeVector.X, 0, 1);

                return relativeVector * targetSize;
            }
        }

        public Vector2 Normalized()
        {
            return vector / vector.Length();
        }

        public Vector2 SafeNormalized(Vector2 fallback)
        {
            if (vector.Length() == 0)
                return fallback;
            return vector.Normalized();
        }

        public PointF ToPointF()
        {
            return new PointF(vector.X, vector.Y);
        }
    }
}
