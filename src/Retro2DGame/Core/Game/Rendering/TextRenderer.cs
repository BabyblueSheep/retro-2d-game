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
            '0' => assets.TextDefault.Zero,
            '1' => assets.TextDefault.One,
            '2' => assets.TextDefault.Two,
            '3' => assets.TextDefault.Three,
            '4' => assets.TextDefault.Four,
            '5' => assets.TextDefault.Five,
            '6' => assets.TextDefault.Six,
            '7' => assets.TextDefault.Seven,
            '8' => assets.TextDefault.Eight,
            '9' => assets.TextDefault.Nine,

            'A' => assets.TextDefault.A,
            'B' => assets.TextDefault.B,
            'C' => assets.TextDefault.C,
            'D' => assets.TextDefault.D,
            'E' => assets.TextDefault.E,
            'F' => assets.TextDefault.F,
            'G' => assets.TextDefault.G,
            'H' => assets.TextDefault.H,
            'I' => assets.TextDefault.I,
            'J' => assets.TextDefault.J,
            'K' => assets.TextDefault.K,
            'L' => assets.TextDefault.L,
            'M' => assets.TextDefault.M,
            'N' => assets.TextDefault.N,
            'O' => assets.TextDefault.O,
            'P' => assets.TextDefault.P,
            'Q' => assets.TextDefault.Q,
            'R' => assets.TextDefault.R,
            'S' => assets.TextDefault.S,
            'T' => assets.TextDefault.T,
            'U' => assets.TextDefault.U,
            'V' => assets.TextDefault.V,
            'W' => assets.TextDefault.W,
            'X' => assets.TextDefault.X,
            'Y' => assets.TextDefault.Y,
            'Z' => assets.TextDefault.Z,

            '.' => assets.TextDefault.Period,
            ',' => assets.TextDefault.Comma,
            '!' => assets.TextDefault.ExclamationMark,
            '?' => assets.TextDefault.QuestionMark,
            '-' => assets.TextDefault.Hyphen,

            _ => assets.TextDefault.QuestionMark,
        };
    }

    private static PaletteIndexBitmap GetTextHighlightedSprite(AssetStorage assets, char character)
    {
        return character switch
        {
            '0' => assets.TextHighlighted.Zero,
            '1' => assets.TextHighlighted.One,
            '2' => assets.TextHighlighted.Two,
            '3' => assets.TextHighlighted.Three,
            '4' => assets.TextHighlighted.Four,
            '5' => assets.TextHighlighted.Five,
            '6' => assets.TextHighlighted.Six,
            '7' => assets.TextHighlighted.Seven,
            '8' => assets.TextHighlighted.Eight,
            '9' => assets.TextHighlighted.Nine,

            'A' => assets.TextHighlighted.A,
            'B' => assets.TextHighlighted.B,
            'C' => assets.TextHighlighted.C,
            'D' => assets.TextHighlighted.D,
            'E' => assets.TextHighlighted.E,
            'F' => assets.TextHighlighted.F,
            'G' => assets.TextHighlighted.G,
            'H' => assets.TextHighlighted.H,
            'I' => assets.TextHighlighted.I,
            'J' => assets.TextHighlighted.J,
            'K' => assets.TextHighlighted.K,
            'L' => assets.TextHighlighted.L,
            'M' => assets.TextHighlighted.M,
            'N' => assets.TextHighlighted.N,
            'O' => assets.TextHighlighted.O,
            'P' => assets.TextHighlighted.P,
            'Q' => assets.TextHighlighted.Q,
            'R' => assets.TextHighlighted.R,
            'S' => assets.TextHighlighted.S,
            'T' => assets.TextHighlighted.T,
            'U' => assets.TextHighlighted.U,
            'V' => assets.TextHighlighted.V,
            'W' => assets.TextHighlighted.W,
            'X' => assets.TextHighlighted.X,
            'Y' => assets.TextHighlighted.Y,
            'Z' => assets.TextHighlighted.Z,

            '.' => assets.TextHighlighted.Period,
            ',' => assets.TextHighlighted.Comma,
            '!' => assets.TextHighlighted.ExclamationMark,
            '?' => assets.TextHighlighted.QuestionMark,
            '-' => assets.TextHighlighted.Hyphen,

            _ => assets.TextHighlighted.QuestionMark,
        };
    }

    private static PaletteIndexBitmap GetTextPressedSprite(AssetStorage assets, char character)
    {
        return character switch
        {
            '0' => assets.TextPressed.Zero,
            '1' => assets.TextPressed.One,
            '2' => assets.TextPressed.Two,
            '3' => assets.TextPressed.Three,
            '4' => assets.TextPressed.Four,
            '5' => assets.TextPressed.Five,
            '6' => assets.TextPressed.Six,
            '7' => assets.TextPressed.Seven,
            '8' => assets.TextPressed.Eight,
            '9' => assets.TextPressed.Nine,

            'A' => assets.TextPressed.A,
            'B' => assets.TextPressed.B,
            'C' => assets.TextPressed.C,
            'D' => assets.TextPressed.D,
            'E' => assets.TextPressed.E,
            'F' => assets.TextPressed.F,
            'G' => assets.TextPressed.G,
            'H' => assets.TextPressed.H,
            'I' => assets.TextPressed.I,
            'J' => assets.TextPressed.J,
            'K' => assets.TextPressed.K,
            'L' => assets.TextPressed.L,
            'M' => assets.TextPressed.M,
            'N' => assets.TextPressed.N,
            'O' => assets.TextPressed.O,
            'P' => assets.TextPressed.P,
            'Q' => assets.TextPressed.Q,
            'R' => assets.TextPressed.R,
            'S' => assets.TextPressed.S,
            'T' => assets.TextPressed.T,
            'U' => assets.TextPressed.U,
            'V' => assets.TextPressed.V,
            'W' => assets.TextPressed.W,
            'X' => assets.TextPressed.X,
            'Y' => assets.TextPressed.Y,
            'Z' => assets.TextPressed.Z,

            '.' => assets.TextPressed.Period,
            ',' => assets.TextPressed.Comma,
            '!' => assets.TextPressed.ExclamationMark,
            '?' => assets.TextPressed.QuestionMark,
            '-' => assets.TextPressed.Hyphen,

            _ => assets.TextPressed.QuestionMark,
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
