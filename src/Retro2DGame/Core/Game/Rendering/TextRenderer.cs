using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class TextRenderer
{
    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }

    private static PaletteIndexBitmap GetTextDefaultSprite(AssetStorage assets, char character)
    {
        return character switch
        {
            '0' => assets.Sprites.TextDefault.Zero,
            '1' => assets.Sprites.TextDefault.One,
            '2' => assets.Sprites.TextDefault.Two,
            '3' => assets.Sprites.TextDefault.Three,
            '4' => assets.Sprites.TextDefault.Four,
            '5' => assets.Sprites.TextDefault.Five,
            '6' => assets.Sprites.TextDefault.Six,
            '7' => assets.Sprites.TextDefault.Seven,
            '8' => assets.Sprites.TextDefault.Eight,
            '9' => assets.Sprites.TextDefault.Nine,

            'A' => assets.Sprites.TextDefault.A,
            'B' => assets.Sprites.TextDefault.B,
            'C' => assets.Sprites.TextDefault.C,
            'D' => assets.Sprites.TextDefault.D,
            'E' => assets.Sprites.TextDefault.E,
            'F' => assets.Sprites.TextDefault.F,
            'G' => assets.Sprites.TextDefault.G,
            'H' => assets.Sprites.TextDefault.H,
            'I' => assets.Sprites.TextDefault.I,
            'J' => assets.Sprites.TextDefault.J,
            'K' => assets.Sprites.TextDefault.K,
            'L' => assets.Sprites.TextDefault.L,
            'M' => assets.Sprites.TextDefault.M,
            'N' => assets.Sprites.TextDefault.N,
            'O' => assets.Sprites.TextDefault.O,
            'P' => assets.Sprites.TextDefault.P,
            'Q' => assets.Sprites.TextDefault.Q,
            'R' => assets.Sprites.TextDefault.R,
            'S' => assets.Sprites.TextDefault.S,
            'T' => assets.Sprites.TextDefault.T,
            'U' => assets.Sprites.TextDefault.U,
            'V' => assets.Sprites.TextDefault.V,
            'W' => assets.Sprites.TextDefault.W,
            'X' => assets.Sprites.TextDefault.X,
            'Y' => assets.Sprites.TextDefault.Y,
            'Z' => assets.Sprites.TextDefault.Z,

            '.' => assets.Sprites.TextDefault.Period,
            ',' => assets.Sprites.TextDefault.Comma,
            '!' => assets.Sprites.TextDefault.ExclamationMark,
            '?' => assets.Sprites.TextDefault.QuestionMark,
            '-' => assets.Sprites.TextDefault.Hyphen,
            ':' => assets.Sprites.TextDefault.Colon,

            _ => assets.Sprites.TextDefault.QuestionMark,
        };
    }

    private static PaletteIndexBitmap GetTextHighlightedSprite(AssetStorage assets, char character)
    {
        return character switch
        {
            '0' => assets.Sprites.TextHighlighted.Zero,
            '1' => assets.Sprites.TextHighlighted.One,
            '2' => assets.Sprites.TextHighlighted.Two,
            '3' => assets.Sprites.TextHighlighted.Three,
            '4' => assets.Sprites.TextHighlighted.Four,
            '5' => assets.Sprites.TextHighlighted.Five,
            '6' => assets.Sprites.TextHighlighted.Six,
            '7' => assets.Sprites.TextHighlighted.Seven,
            '8' => assets.Sprites.TextHighlighted.Eight,
            '9' => assets.Sprites.TextHighlighted.Nine,

            'A' => assets.Sprites.TextHighlighted.A,
            'B' => assets.Sprites.TextHighlighted.B,
            'C' => assets.Sprites.TextHighlighted.C,
            'D' => assets.Sprites.TextHighlighted.D,
            'E' => assets.Sprites.TextHighlighted.E,
            'F' => assets.Sprites.TextHighlighted.F,
            'G' => assets.Sprites.TextHighlighted.G,
            'H' => assets.Sprites.TextHighlighted.H,
            'I' => assets.Sprites.TextHighlighted.I,
            'J' => assets.Sprites.TextHighlighted.J,
            'K' => assets.Sprites.TextHighlighted.K,
            'L' => assets.Sprites.TextHighlighted.L,
            'M' => assets.Sprites.TextHighlighted.M,
            'N' => assets.Sprites.TextHighlighted.N,
            'O' => assets.Sprites.TextHighlighted.O,
            'P' => assets.Sprites.TextHighlighted.P,
            'Q' => assets.Sprites.TextHighlighted.Q,
            'R' => assets.Sprites.TextHighlighted.R,
            'S' => assets.Sprites.TextHighlighted.S,
            'T' => assets.Sprites.TextHighlighted.T,
            'U' => assets.Sprites.TextHighlighted.U,
            'V' => assets.Sprites.TextHighlighted.V,
            'W' => assets.Sprites.TextHighlighted.W,
            'X' => assets.Sprites.TextHighlighted.X,
            'Y' => assets.Sprites.TextHighlighted.Y,
            'Z' => assets.Sprites.TextHighlighted.Z,

            '.' => assets.Sprites.TextHighlighted.Period,
            ',' => assets.Sprites.TextHighlighted.Comma,
            '!' => assets.Sprites.TextHighlighted.ExclamationMark,
            '?' => assets.Sprites.TextHighlighted.QuestionMark,
            '-' => assets.Sprites.TextHighlighted.Hyphen,
            ':' => assets.Sprites.TextHighlighted.Colon,

            _ => assets.Sprites.TextHighlighted.QuestionMark,
        };
    }

    private static PaletteIndexBitmap GetTextPressedSprite(AssetStorage assets, char character)
    {
        return character switch
        {
            '0' => assets.Sprites.TextPressed.Zero,
            '1' => assets.Sprites.TextPressed.One,
            '2' => assets.Sprites.TextPressed.Two,
            '3' => assets.Sprites.TextPressed.Three,
            '4' => assets.Sprites.TextPressed.Four,
            '5' => assets.Sprites.TextPressed.Five,
            '6' => assets.Sprites.TextPressed.Six,
            '7' => assets.Sprites.TextPressed.Seven,
            '8' => assets.Sprites.TextPressed.Eight,
            '9' => assets.Sprites.TextPressed.Nine,

            'A' => assets.Sprites.TextPressed.A,
            'B' => assets.Sprites.TextPressed.B,
            'C' => assets.Sprites.TextPressed.C,
            'D' => assets.Sprites.TextPressed.D,
            'E' => assets.Sprites.TextPressed.E,
            'F' => assets.Sprites.TextPressed.F,
            'G' => assets.Sprites.TextPressed.G,
            'H' => assets.Sprites.TextPressed.H,
            'I' => assets.Sprites.TextPressed.I,
            'J' => assets.Sprites.TextPressed.J,
            'K' => assets.Sprites.TextPressed.K,
            'L' => assets.Sprites.TextPressed.L,
            'M' => assets.Sprites.TextPressed.M,
            'N' => assets.Sprites.TextPressed.N,
            'O' => assets.Sprites.TextPressed.O,
            'P' => assets.Sprites.TextPressed.P,
            'Q' => assets.Sprites.TextPressed.Q,
            'R' => assets.Sprites.TextPressed.R,
            'S' => assets.Sprites.TextPressed.S,
            'T' => assets.Sprites.TextPressed.T,
            'U' => assets.Sprites.TextPressed.U,
            'V' => assets.Sprites.TextPressed.V,
            'W' => assets.Sprites.TextPressed.W,
            'X' => assets.Sprites.TextPressed.X,
            'Y' => assets.Sprites.TextPressed.Y,
            'Z' => assets.Sprites.TextPressed.Z,

            '.' => assets.Sprites.TextPressed.Period,
            ',' => assets.Sprites.TextPressed.Comma,
            '!' => assets.Sprites.TextPressed.ExclamationMark,
            '?' => assets.Sprites.TextPressed.QuestionMark,
            '-' => assets.Sprites.TextPressed.Hyphen,
            ':' => assets.Sprites.TextPressed.Colon,

            _ => assets.Sprites.TextPressed.QuestionMark,
        };
    }

    public static void BlitText
    (
        AssetStorage assets, PaletteIndexBitmap destination,
        int destinationPositionX, int destinationPositionY,
        string text,
        TextAlignment textAlignment = TextAlignment.Left
    )
    {
        int startingPositionX = destinationPositionX;
        int offsetAmount = 8;
        if (textAlignment == TextAlignment.Right)
        {
            var reversedText = "";
            for (int i = text.Length - 1; i >= 0; i--)
            {
                reversedText += text[i];
            }
            text = reversedText;

            offsetAmount = -8;
        }
        if (textAlignment == TextAlignment.Center)
        {
            startingPositionX = destinationPositionX - GetTextWidth(text) / 2;
        }

        text = text.ToUpper();

        var offset = 0;
        var isTextHighlighted = false;
        var isTextSuperHighlighted = false;
        foreach (var character in text)
        {
            if (character == ' ')
            {
                offset++;
                continue;
            }
            if (character == '*')
            {
                isTextHighlighted = !isTextHighlighted;
                continue;
            }
            if (character == '#')
            {
                isTextSuperHighlighted = !isTextSuperHighlighted;
                continue;
            }

            var characterSprite = GetTextDefaultSprite(assets, character);
            if (isTextHighlighted)
                characterSprite = GetTextHighlightedSprite(assets, character);
            if (isTextSuperHighlighted)
                characterSprite = GetTextPressedSprite(assets, character);

            destination.Blit(characterSprite, startingPositionX + offset * offsetAmount, destinationPositionY);
            offset++;
        }
    }

    public static int GetTextWidth(string text)
    {
        var characterAmount = 0;
        foreach (var character in text)
        {
            if (character == '*')
            {
                continue;
            }
            if (character == '#')
            {
                continue;
            }

            characterAmount++;
        }

        return characterAmount * 8;
    }

    public static int GetTextHeight()
    {
        return 8;
    }
}
