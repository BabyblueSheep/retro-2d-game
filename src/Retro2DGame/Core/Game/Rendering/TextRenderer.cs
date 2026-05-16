using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Rendering;

internal sealed class TextRenderer
{
    public GameEngine GameEngine { get; }

    public TextRenderer(GameEngine gameEngine)
    {
        GameEngine = gameEngine;
    }

    private PaletteIndexBitmap GetTextDefaultSprite(char character)
    {
        return character switch
        {
            '0' => GameEngine.AssetStorage.TextDefault0,
            '1' => GameEngine.AssetStorage.TextDefault1,
            '2' => GameEngine.AssetStorage.TextDefault2,
            '3' => GameEngine.AssetStorage.TextDefault3,
            '4' => GameEngine.AssetStorage.TextDefault4,
            '5' => GameEngine.AssetStorage.TextDefault5,
            '6' => GameEngine.AssetStorage.TextDefault6,
            '7' => GameEngine.AssetStorage.TextDefault7,
            '8' => GameEngine.AssetStorage.TextDefault8,
            '9' => GameEngine.AssetStorage.TextDefault9,

            'A' => GameEngine.AssetStorage.TextDefaultA,
            'B' => GameEngine.AssetStorage.TextDefaultB,
            'C' => GameEngine.AssetStorage.TextDefaultC,
            'D' => GameEngine.AssetStorage.TextDefaultD,
            'E' => GameEngine.AssetStorage.TextDefaultE,
            'F' => GameEngine.AssetStorage.TextDefaultF,
            'G' => GameEngine.AssetStorage.TextDefaultG,
            'H' => GameEngine.AssetStorage.TextDefaultH,
            'I' => GameEngine.AssetStorage.TextDefaultI,
            'J' => GameEngine.AssetStorage.TextDefaultJ,
            'K' => GameEngine.AssetStorage.TextDefaultK,
            'L' => GameEngine.AssetStorage.TextDefaultL,
            'M' => GameEngine.AssetStorage.TextDefaultM,
            'N' => GameEngine.AssetStorage.TextDefaultN,
            'O' => GameEngine.AssetStorage.TextDefaultO,
            'P' => GameEngine.AssetStorage.TextDefaultP,
            'Q' => GameEngine.AssetStorage.TextDefaultQ,
            'R' => GameEngine.AssetStorage.TextDefaultR,
            'S' => GameEngine.AssetStorage.TextDefaultS,
            'T' => GameEngine.AssetStorage.TextDefaultT,
            'U' => GameEngine.AssetStorage.TextDefaultU,
            'V' => GameEngine.AssetStorage.TextDefaultV,
            'W' => GameEngine.AssetStorage.TextDefaultW,
            'X' => GameEngine.AssetStorage.TextDefaultX,
            'Y' => GameEngine.AssetStorage.TextDefaultY,
            'Z' => GameEngine.AssetStorage.TextDefaultZ,

            '.' => GameEngine.AssetStorage.TextDefaultPeriod,
            ',' => GameEngine.AssetStorage.TextDefaultComma,
            '!' => GameEngine.AssetStorage.TextDefaultExclamationMark,
            '?' => GameEngine.AssetStorage.TextDefaultQuestionMark,
            '-' => GameEngine.AssetStorage.TextDefaultHyphen,

            _ => GameEngine.AssetStorage.TextHighlightedQuestionMark,
        };
    }

    private PaletteIndexBitmap GetTextHighlightedSprite(char character)
    {
        return character switch
        {
            '0' => GameEngine.AssetStorage.TextHighlighted0,
            '1' => GameEngine.AssetStorage.TextHighlighted1,
            '2' => GameEngine.AssetStorage.TextHighlighted2,
            '3' => GameEngine.AssetStorage.TextHighlighted3,
            '4' => GameEngine.AssetStorage.TextHighlighted4,
            '5' => GameEngine.AssetStorage.TextHighlighted5,
            '6' => GameEngine.AssetStorage.TextHighlighted6,
            '7' => GameEngine.AssetStorage.TextHighlighted7,
            '8' => GameEngine.AssetStorage.TextHighlighted8,
            '9' => GameEngine.AssetStorage.TextHighlighted9,

            'A' => GameEngine.AssetStorage.TextHighlightedA,
            'B' => GameEngine.AssetStorage.TextHighlightedB,
            'C' => GameEngine.AssetStorage.TextHighlightedC,
            'D' => GameEngine.AssetStorage.TextHighlightedD,
            'E' => GameEngine.AssetStorage.TextHighlightedE,
            'F' => GameEngine.AssetStorage.TextHighlightedF,
            'G' => GameEngine.AssetStorage.TextHighlightedG,
            'H' => GameEngine.AssetStorage.TextHighlightedH,
            'I' => GameEngine.AssetStorage.TextHighlightedI,
            'J' => GameEngine.AssetStorage.TextHighlightedJ,
            'K' => GameEngine.AssetStorage.TextHighlightedK,
            'L' => GameEngine.AssetStorage.TextHighlightedL,
            'M' => GameEngine.AssetStorage.TextHighlightedM,
            'N' => GameEngine.AssetStorage.TextHighlightedN,
            'O' => GameEngine.AssetStorage.TextHighlightedO,
            'P' => GameEngine.AssetStorage.TextHighlightedP,
            'Q' => GameEngine.AssetStorage.TextHighlightedQ,
            'R' => GameEngine.AssetStorage.TextHighlightedR,
            'S' => GameEngine.AssetStorage.TextHighlightedS,
            'T' => GameEngine.AssetStorage.TextHighlightedT,
            'U' => GameEngine.AssetStorage.TextHighlightedU,
            'V' => GameEngine.AssetStorage.TextHighlightedV,
            'W' => GameEngine.AssetStorage.TextHighlightedW,
            'X' => GameEngine.AssetStorage.TextHighlightedX,
            'Y' => GameEngine.AssetStorage.TextHighlightedY,
            'Z' => GameEngine.AssetStorage.TextHighlightedZ,

            '.' => GameEngine.AssetStorage.TextHighlightedPeriod,
            ',' => GameEngine.AssetStorage.TextHighlightedComma,
            '!' => GameEngine.AssetStorage.TextHighlightedExclamationMark,
            '?' => GameEngine.AssetStorage.TextHighlightedQuestionMark,
            '-' => GameEngine.AssetStorage.TextHighlightedHyphen,

            _ => GameEngine.AssetStorage.TextHighlightedQuestionMark,
        };
    }

    public void BlitTextDefault
    (
        PaletteIndexBitmap destination,
        uint destinationPositionX, uint destinationPositionY,
        string text
    )
    {
        text = text.ToUpper();

        uint offset = 0;
        var currentlyHighlighted = false;
        foreach (var character in text)
        {
            if (character == ' ')
            {
                offset++;
                continue;
            }
            if (character == '{')
            {
                currentlyHighlighted = true;
                continue;
            }
            if (character == '}')
            {
                currentlyHighlighted = false;
                continue;
            }

            var characterSprite = GetTextDefaultSprite(character);
            if (currentlyHighlighted)
                characterSprite = GetTextHighlightedSprite(character);

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
