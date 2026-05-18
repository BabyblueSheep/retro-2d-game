using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class TextRenderer
{
    private static PaletteIndexBitmap GetTextDefaultSprite(AssetStorage assets, char character)
    {
        return character switch
        {
            '0' => assets.TextDefault0,
            '1' => assets.TextDefault1,
            '2' => assets.TextDefault2,
            '3' => assets.TextDefault3,
            '4' => assets.TextDefault4,
            '5' => assets.TextDefault5,
            '6' => assets.TextDefault6,
            '7' => assets.TextDefault7,
            '8' => assets.TextDefault8,
            '9' => assets.TextDefault9,

            'A' => assets.TextDefaultA,
            'B' => assets.TextDefaultB,
            'C' => assets.TextDefaultC,
            'D' => assets.TextDefaultD,
            'E' => assets.TextDefaultE,
            'F' => assets.TextDefaultF,
            'G' => assets.TextDefaultG,
            'H' => assets.TextDefaultH,
            'I' => assets.TextDefaultI,
            'J' => assets.TextDefaultJ,
            'K' => assets.TextDefaultK,
            'L' => assets.TextDefaultL,
            'M' => assets.TextDefaultM,
            'N' => assets.TextDefaultN,
            'O' => assets.TextDefaultO,
            'P' => assets.TextDefaultP,
            'Q' => assets.TextDefaultQ,
            'R' => assets.TextDefaultR,
            'S' => assets.TextDefaultS,
            'T' => assets.TextDefaultT,
            'U' => assets.TextDefaultU,
            'V' => assets.TextDefaultV,
            'W' => assets.TextDefaultW,
            'X' => assets.TextDefaultX,
            'Y' => assets.TextDefaultY,
            'Z' => assets.TextDefaultZ,

            '.' => assets.TextDefaultPeriod,
            ',' => assets.TextDefaultComma,
            '!' => assets.TextDefaultExclamationMark,
            '?' => assets.TextDefaultQuestionMark,
            '-' => assets.TextDefaultHyphen,

            _ => assets.TextHighlightedQuestionMark,
        };
    }

    private static PaletteIndexBitmap GetTextHighlightedSprite(AssetStorage assets, char character)
    {
        return character switch
        {
            '0' => assets.TextHighlighted0,
            '1' => assets.TextHighlighted1,
            '2' => assets.TextHighlighted2,
            '3' => assets.TextHighlighted3,
            '4' => assets.TextHighlighted4,
            '5' => assets.TextHighlighted5,
            '6' => assets.TextHighlighted6,
            '7' => assets.TextHighlighted7,
            '8' => assets.TextHighlighted8,
            '9' => assets.TextHighlighted9,

            'A' => assets.TextHighlightedA,
            'B' => assets.TextHighlightedB,
            'C' => assets.TextHighlightedC,
            'D' => assets.TextHighlightedD,
            'E' => assets.TextHighlightedE,
            'F' => assets.TextHighlightedF,
            'G' => assets.TextHighlightedG,
            'H' => assets.TextHighlightedH,
            'I' => assets.TextHighlightedI,
            'J' => assets.TextHighlightedJ,
            'K' => assets.TextHighlightedK,
            'L' => assets.TextHighlightedL,
            'M' => assets.TextHighlightedM,
            'N' => assets.TextHighlightedN,
            'O' => assets.TextHighlightedO,
            'P' => assets.TextHighlightedP,
            'Q' => assets.TextHighlightedQ,
            'R' => assets.TextHighlightedR,
            'S' => assets.TextHighlightedS,
            'T' => assets.TextHighlightedT,
            'U' => assets.TextHighlightedU,
            'V' => assets.TextHighlightedV,
            'W' => assets.TextHighlightedW,
            'X' => assets.TextHighlightedX,
            'Y' => assets.TextHighlightedY,
            'Z' => assets.TextHighlightedZ,

            '.' => assets.TextHighlightedPeriod,
            ',' => assets.TextHighlightedComma,
            '!' => assets.TextHighlightedExclamationMark,
            '?' => assets.TextHighlightedQuestionMark,
            '-' => assets.TextHighlightedHyphen,

            _ => assets.TextHighlightedQuestionMark,
        };
    }

    private static PaletteIndexBitmap GetTextPressedSprite(AssetStorage assets, char character)
    {
        return character switch
        {
            '0' => assets.TextPressed0,
            '1' => assets.TextPressed1,
            '2' => assets.TextPressed2,
            '3' => assets.TextPressed3,
            '4' => assets.TextPressed4,
            '5' => assets.TextPressed5,
            '6' => assets.TextPressed6,
            '7' => assets.TextPressed7,
            '8' => assets.TextPressed8,
            '9' => assets.TextPressed9,

            'A' => assets.TextPressedA,
            'B' => assets.TextPressedB,
            'C' => assets.TextPressedC,
            'D' => assets.TextPressedD,
            'E' => assets.TextPressedE,
            'F' => assets.TextPressedF,
            'G' => assets.TextPressedG,
            'H' => assets.TextPressedH,
            'I' => assets.TextPressedI,
            'J' => assets.TextPressedJ,
            'K' => assets.TextPressedK,
            'L' => assets.TextPressedL,
            'M' => assets.TextPressedM,
            'N' => assets.TextPressedN,
            'O' => assets.TextPressedO,
            'P' => assets.TextPressedP,
            'Q' => assets.TextPressedQ,
            'R' => assets.TextPressedR,
            'S' => assets.TextPressedS,
            'T' => assets.TextPressedT,
            'U' => assets.TextPressedU,
            'V' => assets.TextPressedV,
            'W' => assets.TextPressedW,
            'X' => assets.TextPressedX,
            'Y' => assets.TextPressedY,
            'Z' => assets.TextPressedZ,

            '.' => assets.TextPressedPeriod,
            ',' => assets.TextPressedComma,
            '!' => assets.TextPressedExclamationMark,
            '?' => assets.TextPressedQuestionMark,
            '-' => assets.TextPressedHyphen,

            _ => assets.TextPressedQuestionMark,
        };
    }

    public static void BlitText
    (
        AssetStorage assets, PaletteIndexBitmap destination,
        uint destinationPositionX, uint destinationPositionY,
        string text
    )
    {
        text = text.ToUpper();

        uint offset = 0;
        var textColor = 0;
        foreach (var character in text)
        {
            if (character == ' ')
            {
                offset++;
                continue;
            }
            if (character == '{')
            {
                textColor++;
                continue;
            }
            if (character == '}')
            {
                textColor--;
                continue;
            }

            var characterSprite = GetTextDefaultSprite(assets, character);
            if (textColor == 1)
                characterSprite = GetTextHighlightedSprite(assets, character);
            if (textColor == 2)
                characterSprite = GetTextPressedSprite(assets, character);

            destination.Blit(characterSprite, destinationPositionX + offset * 8, destinationPositionY);
            offset++;
        }
    }

    public static int GetTextWidth(string text)
    {
        return text.Length * 8;
    }

    public static int GetTextHeight()
    {
        return 8;
    }
}
