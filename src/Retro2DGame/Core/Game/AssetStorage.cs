using Retro2DGame.Core.Game.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Retro2DGame.Core.Game;

[AttributeUsage(AttributeTargets.Property)]
file class SpriteFrameAttribute : Attribute
{
    public string SpriteName { get; }
    public Rectangle Frame { get; }

    public SpriteFrameAttribute(string spriteName, int[] frame)
    {
        SpriteName = spriteName;
        Frame = new Rectangle(frame[0], frame[1], frame[2], frame[3]);
    }

    public SpriteFrameAttribute(string spriteName, int[] frameIndex, int[] frameSize)
    {
        SpriteName = spriteName;
        Frame = new Rectangle(frameIndex[0] * frameSize[0], frameIndex[1] * frameSize[1], frameSize[0], frameSize[1]);
    }
}

internal sealed class AssetStorage
{
    #region player
    [SpriteFrame("player", [0, 0], [16, 16])] public PaletteIndexBitmap PlayerIdle { get; private set; }

    [SpriteFrame("player", [1, 0], [16, 16])] public PaletteIndexBitmap PlayerMove1 { get; private set; }
    [SpriteFrame("player", [1, 1], [16, 16])] public PaletteIndexBitmap PlayerMove2 { get; private set; }
    [SpriteFrame("player", [1, 2], [16, 16])] public PaletteIndexBitmap PlayerMove3 { get; private set; }
    #endregion

    #region text_default
    [SpriteFrame("text_default", [0, 0], [8, 8])] public PaletteIndexBitmap TextDefault0 { get; private set; }
    [SpriteFrame("text_default", [1, 0], [8, 8])] public PaletteIndexBitmap TextDefault1 { get; private set; }
    [SpriteFrame("text_default", [2, 0], [8, 8])] public PaletteIndexBitmap TextDefault2 { get; private set; }
    [SpriteFrame("text_default", [3, 0], [8, 8])] public PaletteIndexBitmap TextDefault3 { get; private set; }
    [SpriteFrame("text_default", [4, 0], [8, 8])] public PaletteIndexBitmap TextDefault4 { get; private set; }
    [SpriteFrame("text_default", [5, 0], [8, 8])] public PaletteIndexBitmap TextDefault5 { get; private set; }
    [SpriteFrame("text_default", [6, 0], [8, 8])] public PaletteIndexBitmap TextDefault6 { get; private set; }
    [SpriteFrame("text_default", [7, 0], [8, 8])] public PaletteIndexBitmap TextDefault7 { get; private set; }
    [SpriteFrame("text_default", [8, 0], [8, 8])] public PaletteIndexBitmap TextDefault8 { get; private set; }
    [SpriteFrame("text_default", [9, 0], [8, 8])] public PaletteIndexBitmap TextDefault9 { get; private set; }

    [SpriteFrame("text_default", [0, 1], [8, 8])] public PaletteIndexBitmap TextDefaultA { get; private set; }
    [SpriteFrame("text_default", [1, 1], [8, 8])] public PaletteIndexBitmap TextDefaultB { get; private set; }
    [SpriteFrame("text_default", [2, 1], [8, 8])] public PaletteIndexBitmap TextDefaultC { get; private set; }
    [SpriteFrame("text_default", [3, 1], [8, 8])] public PaletteIndexBitmap TextDefaultD { get; private set; }
    [SpriteFrame("text_default", [4, 1], [8, 8])] public PaletteIndexBitmap TextDefaultE { get; private set; }
    [SpriteFrame("text_default", [5, 1], [8, 8])] public PaletteIndexBitmap TextDefaultF { get; private set; }
    [SpriteFrame("text_default", [6, 1], [8, 8])] public PaletteIndexBitmap TextDefaultG { get; private set; }
    [SpriteFrame("text_default", [7, 1], [8, 8])] public PaletteIndexBitmap TextDefaultH { get; private set; }
    [SpriteFrame("text_default", [8, 1], [8, 8])] public PaletteIndexBitmap TextDefaultI { get; private set; }
    [SpriteFrame("text_default", [9, 1], [8, 8])] public PaletteIndexBitmap TextDefaultJ { get; private set; }

    [SpriteFrame("text_default", [0, 2], [8, 8])] public PaletteIndexBitmap TextDefaultK { get; private set; }
    [SpriteFrame("text_default", [1, 2], [8, 8])] public PaletteIndexBitmap TextDefaultL { get; private set; }
    [SpriteFrame("text_default", [2, 2], [8, 8])] public PaletteIndexBitmap TextDefaultM { get; private set; }
    [SpriteFrame("text_default", [3, 2], [8, 8])] public PaletteIndexBitmap TextDefaultN { get; private set; }
    [SpriteFrame("text_default", [4, 2], [8, 8])] public PaletteIndexBitmap TextDefaultO { get; private set; }
    [SpriteFrame("text_default", [5, 2], [8, 8])] public PaletteIndexBitmap TextDefaultP { get; private set; }
    [SpriteFrame("text_default", [6, 2], [8, 8])] public PaletteIndexBitmap TextDefaultQ { get; private set; }
    [SpriteFrame("text_default", [7, 2], [8, 8])] public PaletteIndexBitmap TextDefaultR { get; private set; }
    [SpriteFrame("text_default", [8, 2], [8, 8])] public PaletteIndexBitmap TextDefaultS { get; private set; }
    [SpriteFrame("text_default", [9, 2], [8, 8])] public PaletteIndexBitmap TextDefaultT { get; private set; }

    [SpriteFrame("text_default", [0, 3], [8, 8])] public PaletteIndexBitmap TextDefaultU { get; private set; }
    [SpriteFrame("text_default", [1, 3], [8, 8])] public PaletteIndexBitmap TextDefaultV { get; private set; }
    [SpriteFrame("text_default", [2, 3], [8, 8])] public PaletteIndexBitmap TextDefaultW { get; private set; }
    [SpriteFrame("text_default", [3, 3], [8, 8])] public PaletteIndexBitmap TextDefaultX { get; private set; }
    [SpriteFrame("text_default", [4, 3], [8, 8])] public PaletteIndexBitmap TextDefaultY { get; private set; }
    [SpriteFrame("text_default", [5, 3], [8, 8])] public PaletteIndexBitmap TextDefaultZ { get; private set; }

    [SpriteFrame("text_default", [0, 4], [8, 8])] public PaletteIndexBitmap TextDefaultComma { get; private set; }
    [SpriteFrame("text_default", [1, 4], [8, 8])] public PaletteIndexBitmap TextDefaultExclamationMark { get; private set; }
    [SpriteFrame("text_default", [2, 4], [8, 8])] public PaletteIndexBitmap TextDefaultPeriod { get; private set; }
    [SpriteFrame("text_default", [3, 4], [8, 8])] public PaletteIndexBitmap TextDefaultQuestionMark { get; private set; }
    [SpriteFrame("text_default", [4, 4], [8, 8])] public PaletteIndexBitmap TextDefaultHyphen { get; private set; }
    #endregion

    #region text_highlighted
    [SpriteFrame("text_highlighted", [0, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted0 { get; private set; }
    [SpriteFrame("text_highlighted", [1, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted1 { get; private set; }
    [SpriteFrame("text_highlighted", [2, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted2 { get; private set; }
    [SpriteFrame("text_highlighted", [3, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted3 { get; private set; }
    [SpriteFrame("text_highlighted", [4, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted4 { get; private set; }
    [SpriteFrame("text_highlighted", [5, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted5 { get; private set; }
    [SpriteFrame("text_highlighted", [6, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted6 { get; private set; }
    [SpriteFrame("text_highlighted", [7, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted7 { get; private set; }
    [SpriteFrame("text_highlighted", [8, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted8 { get; private set; }
    [SpriteFrame("text_highlighted", [9, 0], [8, 8])] public PaletteIndexBitmap TextHighlighted9 { get; private set; }

    [SpriteFrame("text_highlighted", [0, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedA { get; private set; }
    [SpriteFrame("text_highlighted", [1, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedB { get; private set; }
    [SpriteFrame("text_highlighted", [2, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedC { get; private set; }
    [SpriteFrame("text_highlighted", [3, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedD { get; private set; }
    [SpriteFrame("text_highlighted", [4, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedE { get; private set; }
    [SpriteFrame("text_highlighted", [5, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedF { get; private set; }
    [SpriteFrame("text_highlighted", [6, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedG { get; private set; }
    [SpriteFrame("text_highlighted", [7, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedH { get; private set; }
    [SpriteFrame("text_highlighted", [8, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedI { get; private set; }
    [SpriteFrame("text_highlighted", [9, 1], [8, 8])] public PaletteIndexBitmap TextHighlightedJ { get; private set; }

    [SpriteFrame("text_highlighted", [0, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedK { get; private set; }
    [SpriteFrame("text_highlighted", [1, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedL { get; private set; }
    [SpriteFrame("text_highlighted", [2, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedM { get; private set; }
    [SpriteFrame("text_highlighted", [3, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedN { get; private set; }
    [SpriteFrame("text_highlighted", [4, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedO { get; private set; }
    [SpriteFrame("text_highlighted", [5, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedP { get; private set; }
    [SpriteFrame("text_highlighted", [6, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedQ { get; private set; }
    [SpriteFrame("text_highlighted", [7, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedR { get; private set; }
    [SpriteFrame("text_highlighted", [8, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedS { get; private set; }
    [SpriteFrame("text_highlighted", [9, 2], [8, 8])] public PaletteIndexBitmap TextHighlightedT { get; private set; }

    [SpriteFrame("text_highlighted", [0, 3], [8, 8])] public PaletteIndexBitmap TextHighlightedU { get; private set; }
    [SpriteFrame("text_highlighted", [1, 3], [8, 8])] public PaletteIndexBitmap TextHighlightedV { get; private set; }
    [SpriteFrame("text_highlighted", [2, 3], [8, 8])] public PaletteIndexBitmap TextHighlightedW { get; private set; }
    [SpriteFrame("text_highlighted", [3, 3], [8, 8])] public PaletteIndexBitmap TextHighlightedX { get; private set; }
    [SpriteFrame("text_highlighted", [4, 3], [8, 8])] public PaletteIndexBitmap TextHighlightedY { get; private set; }
    [SpriteFrame("text_highlighted", [5, 3], [8, 8])] public PaletteIndexBitmap TextHighlightedZ { get; private set; }

    [SpriteFrame("text_highlighted", [0, 4], [8, 8])] public PaletteIndexBitmap TextHighlightedComma { get; private set; }
    [SpriteFrame("text_highlighted", [1, 4], [8, 8])] public PaletteIndexBitmap TextHighlightedExclamationMark { get; private set; }
    [SpriteFrame("text_highlighted", [2, 4], [8, 8])] public PaletteIndexBitmap TextHighlightedPeriod { get; private set; }
    [SpriteFrame("text_highlighted", [3, 4], [8, 8])] public PaletteIndexBitmap TextHighlightedQuestionMark { get; private set; }
    [SpriteFrame("text_highlighted", [4, 4], [8, 8])] public PaletteIndexBitmap TextHighlightedHyphen { get; private set; }
    #endregion

    #region text_pressed
    [SpriteFrame("text_pressed", [0, 0], [8, 8])] public PaletteIndexBitmap TextPressed0 { get; private set; }
    [SpriteFrame("text_pressed", [1, 0], [8, 8])] public PaletteIndexBitmap TextPressed1 { get; private set; }
    [SpriteFrame("text_pressed", [2, 0], [8, 8])] public PaletteIndexBitmap TextPressed2 { get; private set; }
    [SpriteFrame("text_pressed", [3, 0], [8, 8])] public PaletteIndexBitmap TextPressed3 { get; private set; }
    [SpriteFrame("text_pressed", [4, 0], [8, 8])] public PaletteIndexBitmap TextPressed4 { get; private set; }
    [SpriteFrame("text_pressed", [5, 0], [8, 8])] public PaletteIndexBitmap TextPressed5 { get; private set; }
    [SpriteFrame("text_pressed", [6, 0], [8, 8])] public PaletteIndexBitmap TextPressed6 { get; private set; }
    [SpriteFrame("text_pressed", [7, 0], [8, 8])] public PaletteIndexBitmap TextPressed7 { get; private set; }
    [SpriteFrame("text_pressed", [8, 0], [8, 8])] public PaletteIndexBitmap TextPressed8 { get; private set; }
    [SpriteFrame("text_pressed", [9, 0], [8, 8])] public PaletteIndexBitmap TextPressed9 { get; private set; }

    [SpriteFrame("text_pressed", [0, 1], [8, 8])] public PaletteIndexBitmap TextPressedA { get; private set; }
    [SpriteFrame("text_pressed", [1, 1], [8, 8])] public PaletteIndexBitmap TextPressedB { get; private set; }
    [SpriteFrame("text_pressed", [2, 1], [8, 8])] public PaletteIndexBitmap TextPressedC { get; private set; }
    [SpriteFrame("text_pressed", [3, 1], [8, 8])] public PaletteIndexBitmap TextPressedD { get; private set; }
    [SpriteFrame("text_pressed", [4, 1], [8, 8])] public PaletteIndexBitmap TextPressedE { get; private set; }
    [SpriteFrame("text_pressed", [5, 1], [8, 8])] public PaletteIndexBitmap TextPressedF { get; private set; }
    [SpriteFrame("text_pressed", [6, 1], [8, 8])] public PaletteIndexBitmap TextPressedG { get; private set; }
    [SpriteFrame("text_pressed", [7, 1], [8, 8])] public PaletteIndexBitmap TextPressedH { get; private set; }
    [SpriteFrame("text_pressed", [8, 1], [8, 8])] public PaletteIndexBitmap TextPressedI { get; private set; }
    [SpriteFrame("text_pressed", [9, 1], [8, 8])] public PaletteIndexBitmap TextPressedJ { get; private set; }

    [SpriteFrame("text_pressed", [0, 2], [8, 8])] public PaletteIndexBitmap TextPressedK { get; private set; }
    [SpriteFrame("text_pressed", [1, 2], [8, 8])] public PaletteIndexBitmap TextPressedL { get; private set; }
    [SpriteFrame("text_pressed", [2, 2], [8, 8])] public PaletteIndexBitmap TextPressedM { get; private set; }
    [SpriteFrame("text_pressed", [3, 2], [8, 8])] public PaletteIndexBitmap TextPressedN { get; private set; }
    [SpriteFrame("text_pressed", [4, 2], [8, 8])] public PaletteIndexBitmap TextPressedO { get; private set; }
    [SpriteFrame("text_pressed", [5, 2], [8, 8])] public PaletteIndexBitmap TextPressedP { get; private set; }
    [SpriteFrame("text_pressed", [6, 2], [8, 8])] public PaletteIndexBitmap TextPressedQ { get; private set; }
    [SpriteFrame("text_pressed", [7, 2], [8, 8])] public PaletteIndexBitmap TextPressedR { get; private set; }
    [SpriteFrame("text_pressed", [8, 2], [8, 8])] public PaletteIndexBitmap TextPressedS { get; private set; }
    [SpriteFrame("text_pressed", [9, 2], [8, 8])] public PaletteIndexBitmap TextPressedT { get; private set; }

    [SpriteFrame("text_pressed", [0, 3], [8, 8])] public PaletteIndexBitmap TextPressedU { get; private set; }
    [SpriteFrame("text_pressed", [1, 3], [8, 8])] public PaletteIndexBitmap TextPressedV { get; private set; }
    [SpriteFrame("text_pressed", [2, 3], [8, 8])] public PaletteIndexBitmap TextPressedW { get; private set; }
    [SpriteFrame("text_pressed", [3, 3], [8, 8])] public PaletteIndexBitmap TextPressedX { get; private set; }
    [SpriteFrame("text_pressed", [4, 3], [8, 8])] public PaletteIndexBitmap TextPressedY { get; private set; }
    [SpriteFrame("text_pressed", [5, 3], [8, 8])] public PaletteIndexBitmap TextPressedZ { get; private set; }

    [SpriteFrame("text_pressed", [0, 4], [8, 8])] public PaletteIndexBitmap TextPressedComma { get; private set; }
    [SpriteFrame("text_pressed", [1, 4], [8, 8])] public PaletteIndexBitmap TextPressedExclamationMark { get; private set; }
    [SpriteFrame("text_pressed", [2, 4], [8, 8])] public PaletteIndexBitmap TextPressedPeriod { get; private set; }
    [SpriteFrame("text_pressed", [3, 4], [8, 8])] public PaletteIndexBitmap TextPressedQuestionMark { get; private set; }
    [SpriteFrame("text_pressed", [4, 4], [8, 8])] public PaletteIndexBitmap TextPressedHyphen { get; private set; }
    #endregion




    #region Loading
#pragma warning disable CS8618
    public AssetStorage()
#pragma warning restore CS8618
    {
        Dictionary<string, PaletteIndexBitmap> bitmaps = new()
        {
            ["player"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\player.ptid"),
            ["text_default"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\text_default.ptid"),
            ["text_highlighted"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\text_highlighted.ptid"),
            ["text_pressed"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\text_pressed.ptid"),
        };

        var assetStorageType = GetType();
        var frameAttributeType = typeof(SpriteFrameAttribute);
        foreach (var property in assetStorageType.GetProperties())
        {
            var attribute = property.GetCustomAttribute(frameAttributeType);
            if (attribute is not SpriteFrameAttribute frameAttribute)
                continue;

            var newBitmap = PaletteIndexBitmap.CreateEmpty((uint)frameAttribute.Frame.Width, (uint)frameAttribute.Frame.Height);
            newBitmap.Blit(bitmaps[frameAttribute.SpriteName], 0, 0, frameAttribute.Frame);
            property.SetValue(this, newBitmap);
        }
    }
    #endregion
}
