using Retro2DGame.Core.Game.Rendering;
using Retro2DGame.Core.SDL3;
using SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Retro2DGame.Core.Game;

#pragma warning disable CS8618

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

file interface IHasSpritesToLoad { }

internal sealed class AssetStorage
{
    public sealed class SpriteStorage : IDisposable
    {
        public sealed class BackgroundSpriteStorage : IHasSpritesToLoad
        {
            [SpriteFrame("generic_background", [0, 0], [256, 256])] public PaletteIndexBitmap Generic { get; private set; }
        }
        public BackgroundSpriteStorage Background { get; } = new BackgroundSpriteStorage();

        public sealed class PlayerSpritesStorage : IHasSpritesToLoad
        {
            [SpriteFrame("player", [0, 0], [16, 16])] public PaletteIndexBitmap Idle { get; private set; }

            [SpriteFrame("player", [1, 0], [16, 16])] public PaletteIndexBitmap Move1 { get; private set; }
            [SpriteFrame("player", [1, 1], [16, 16])] public PaletteIndexBitmap Move2 { get; private set; }
            [SpriteFrame("player", [1, 2], [16, 16])] public PaletteIndexBitmap Move3 { get; private set; }

            [SpriteFrame("lantern", [0, 0], [16, 16])] public PaletteIndexBitmap Lantern { get; private set; }
        }
        public PlayerSpritesStorage Player { get; } = new PlayerSpritesStorage();

        public sealed class EnemiesSpritesStorage : IHasSpritesToLoad
        {
            [SpriteFrame("enemies", [0, 0], [16, 16])] public PaletteIndexBitmap GhostGeneric { get; private set; }
            [SpriteFrame("enemies", [1, 0], [16, 16])] public PaletteIndexBitmap GhostTeleporting { get; private set; }
            [SpriteFrame("enemies", [0, 1], [16, 16])] public PaletteIndexBitmap GhostDrunk { get; private set; }
            [SpriteFrame("enemies", [32, 0, 32, 32])] public PaletteIndexBitmap GhostBrute { get; private set; }
        }
        public EnemiesSpritesStorage Enemies { get; } = new EnemiesSpritesStorage();

        public sealed class TextDefaultSpriteStorage : IHasSpritesToLoad
        {
            [SpriteFrame("text_default", [0, 0], [8, 8])] public PaletteIndexBitmap Zero { get; private set; }
            [SpriteFrame("text_default", [1, 0], [8, 8])] public PaletteIndexBitmap One { get; private set; }
            [SpriteFrame("text_default", [2, 0], [8, 8])] public PaletteIndexBitmap Two { get; private set; }
            [SpriteFrame("text_default", [3, 0], [8, 8])] public PaletteIndexBitmap Three { get; private set; }
            [SpriteFrame("text_default", [4, 0], [8, 8])] public PaletteIndexBitmap Four { get; private set; }
            [SpriteFrame("text_default", [5, 0], [8, 8])] public PaletteIndexBitmap Five { get; private set; }
            [SpriteFrame("text_default", [6, 0], [8, 8])] public PaletteIndexBitmap Six { get; private set; }
            [SpriteFrame("text_default", [7, 0], [8, 8])] public PaletteIndexBitmap Seven { get; private set; }
            [SpriteFrame("text_default", [8, 0], [8, 8])] public PaletteIndexBitmap Eight { get; private set; }
            [SpriteFrame("text_default", [9, 0], [8, 8])] public PaletteIndexBitmap Nine { get; private set; }

            [SpriteFrame("text_default", [0, 1], [8, 8])] public PaletteIndexBitmap A { get; private set; }
            [SpriteFrame("text_default", [1, 1], [8, 8])] public PaletteIndexBitmap B { get; private set; }
            [SpriteFrame("text_default", [2, 1], [8, 8])] public PaletteIndexBitmap C { get; private set; }
            [SpriteFrame("text_default", [3, 1], [8, 8])] public PaletteIndexBitmap D { get; private set; }
            [SpriteFrame("text_default", [4, 1], [8, 8])] public PaletteIndexBitmap E { get; private set; }
            [SpriteFrame("text_default", [5, 1], [8, 8])] public PaletteIndexBitmap F { get; private set; }
            [SpriteFrame("text_default", [6, 1], [8, 8])] public PaletteIndexBitmap G { get; private set; }
            [SpriteFrame("text_default", [7, 1], [8, 8])] public PaletteIndexBitmap H { get; private set; }
            [SpriteFrame("text_default", [8, 1], [8, 8])] public PaletteIndexBitmap I { get; private set; }
            [SpriteFrame("text_default", [9, 1], [8, 8])] public PaletteIndexBitmap J { get; private set; }

            [SpriteFrame("text_default", [0, 2], [8, 8])] public PaletteIndexBitmap K { get; private set; }
            [SpriteFrame("text_default", [1, 2], [8, 8])] public PaletteIndexBitmap L { get; private set; }
            [SpriteFrame("text_default", [2, 2], [8, 8])] public PaletteIndexBitmap M { get; private set; }
            [SpriteFrame("text_default", [3, 2], [8, 8])] public PaletteIndexBitmap N { get; private set; }
            [SpriteFrame("text_default", [4, 2], [8, 8])] public PaletteIndexBitmap O { get; private set; }
            [SpriteFrame("text_default", [5, 2], [8, 8])] public PaletteIndexBitmap P { get; private set; }
            [SpriteFrame("text_default", [6, 2], [8, 8])] public PaletteIndexBitmap Q { get; private set; }
            [SpriteFrame("text_default", [7, 2], [8, 8])] public PaletteIndexBitmap R { get; private set; }
            [SpriteFrame("text_default", [8, 2], [8, 8])] public PaletteIndexBitmap S { get; private set; }
            [SpriteFrame("text_default", [9, 2], [8, 8])] public PaletteIndexBitmap T { get; private set; }

            [SpriteFrame("text_default", [0, 3], [8, 8])] public PaletteIndexBitmap U { get; private set; }
            [SpriteFrame("text_default", [1, 3], [8, 8])] public PaletteIndexBitmap V { get; private set; }
            [SpriteFrame("text_default", [2, 3], [8, 8])] public PaletteIndexBitmap W { get; private set; }
            [SpriteFrame("text_default", [3, 3], [8, 8])] public PaletteIndexBitmap X { get; private set; }
            [SpriteFrame("text_default", [4, 3], [8, 8])] public PaletteIndexBitmap Y { get; private set; }
            [SpriteFrame("text_default", [5, 3], [8, 8])] public PaletteIndexBitmap Z { get; private set; }

            [SpriteFrame("text_default", [0, 4], [8, 8])] public PaletteIndexBitmap Comma { get; private set; }
            [SpriteFrame("text_default", [1, 4], [8, 8])] public PaletteIndexBitmap ExclamationMark { get; private set; }
            [SpriteFrame("text_default", [2, 4], [8, 8])] public PaletteIndexBitmap Period { get; private set; }
            [SpriteFrame("text_default", [3, 4], [8, 8])] public PaletteIndexBitmap QuestionMark { get; private set; }
            [SpriteFrame("text_default", [4, 4], [8, 8])] public PaletteIndexBitmap Hyphen { get; private set; }
            [SpriteFrame("text_default", [5, 4], [8, 8])] public PaletteIndexBitmap Colon { get; private set; }
        }
        public TextDefaultSpriteStorage TextDefault { get; } = new TextDefaultSpriteStorage();

        public sealed class TextHighlightedSpriteStorage : IHasSpritesToLoad
        {
            [SpriteFrame("text_highlighted", [0, 0], [8, 8])] public PaletteIndexBitmap Zero { get; private set; }
            [SpriteFrame("text_highlighted", [1, 0], [8, 8])] public PaletteIndexBitmap One { get; private set; }
            [SpriteFrame("text_highlighted", [2, 0], [8, 8])] public PaletteIndexBitmap Two { get; private set; }
            [SpriteFrame("text_highlighted", [3, 0], [8, 8])] public PaletteIndexBitmap Three { get; private set; }
            [SpriteFrame("text_highlighted", [4, 0], [8, 8])] public PaletteIndexBitmap Four { get; private set; }
            [SpriteFrame("text_highlighted", [5, 0], [8, 8])] public PaletteIndexBitmap Five { get; private set; }
            [SpriteFrame("text_highlighted", [6, 0], [8, 8])] public PaletteIndexBitmap Six { get; private set; }
            [SpriteFrame("text_highlighted", [7, 0], [8, 8])] public PaletteIndexBitmap Seven { get; private set; }
            [SpriteFrame("text_highlighted", [8, 0], [8, 8])] public PaletteIndexBitmap Eight { get; private set; }
            [SpriteFrame("text_highlighted", [9, 0], [8, 8])] public PaletteIndexBitmap Nine { get; private set; }

            [SpriteFrame("text_highlighted", [0, 1], [8, 8])] public PaletteIndexBitmap A { get; private set; }
            [SpriteFrame("text_highlighted", [1, 1], [8, 8])] public PaletteIndexBitmap B { get; private set; }
            [SpriteFrame("text_highlighted", [2, 1], [8, 8])] public PaletteIndexBitmap C { get; private set; }
            [SpriteFrame("text_highlighted", [3, 1], [8, 8])] public PaletteIndexBitmap D { get; private set; }
            [SpriteFrame("text_highlighted", [4, 1], [8, 8])] public PaletteIndexBitmap E { get; private set; }
            [SpriteFrame("text_highlighted", [5, 1], [8, 8])] public PaletteIndexBitmap F { get; private set; }
            [SpriteFrame("text_highlighted", [6, 1], [8, 8])] public PaletteIndexBitmap G { get; private set; }
            [SpriteFrame("text_highlighted", [7, 1], [8, 8])] public PaletteIndexBitmap H { get; private set; }
            [SpriteFrame("text_highlighted", [8, 1], [8, 8])] public PaletteIndexBitmap I { get; private set; }
            [SpriteFrame("text_highlighted", [9, 1], [8, 8])] public PaletteIndexBitmap J { get; private set; }

            [SpriteFrame("text_highlighted", [0, 2], [8, 8])] public PaletteIndexBitmap K { get; private set; }
            [SpriteFrame("text_highlighted", [1, 2], [8, 8])] public PaletteIndexBitmap L { get; private set; }
            [SpriteFrame("text_highlighted", [2, 2], [8, 8])] public PaletteIndexBitmap M { get; private set; }
            [SpriteFrame("text_highlighted", [3, 2], [8, 8])] public PaletteIndexBitmap N { get; private set; }
            [SpriteFrame("text_highlighted", [4, 2], [8, 8])] public PaletteIndexBitmap O { get; private set; }
            [SpriteFrame("text_highlighted", [5, 2], [8, 8])] public PaletteIndexBitmap P { get; private set; }
            [SpriteFrame("text_highlighted", [6, 2], [8, 8])] public PaletteIndexBitmap Q { get; private set; }
            [SpriteFrame("text_highlighted", [7, 2], [8, 8])] public PaletteIndexBitmap R { get; private set; }
            [SpriteFrame("text_highlighted", [8, 2], [8, 8])] public PaletteIndexBitmap S { get; private set; }
            [SpriteFrame("text_highlighted", [9, 2], [8, 8])] public PaletteIndexBitmap T { get; private set; }

            [SpriteFrame("text_highlighted", [0, 3], [8, 8])] public PaletteIndexBitmap U { get; private set; }
            [SpriteFrame("text_highlighted", [1, 3], [8, 8])] public PaletteIndexBitmap V { get; private set; }
            [SpriteFrame("text_highlighted", [2, 3], [8, 8])] public PaletteIndexBitmap W { get; private set; }
            [SpriteFrame("text_highlighted", [3, 3], [8, 8])] public PaletteIndexBitmap X { get; private set; }
            [SpriteFrame("text_highlighted", [4, 3], [8, 8])] public PaletteIndexBitmap Y { get; private set; }
            [SpriteFrame("text_highlighted", [5, 3], [8, 8])] public PaletteIndexBitmap Z { get; private set; }

            [SpriteFrame("text_highlighted", [0, 4], [8, 8])] public PaletteIndexBitmap Comma { get; private set; }
            [SpriteFrame("text_highlighted", [1, 4], [8, 8])] public PaletteIndexBitmap ExclamationMark { get; private set; }
            [SpriteFrame("text_highlighted", [2, 4], [8, 8])] public PaletteIndexBitmap Period { get; private set; }
            [SpriteFrame("text_highlighted", [3, 4], [8, 8])] public PaletteIndexBitmap QuestionMark { get; private set; }
            [SpriteFrame("text_highlighted", [4, 4], [8, 8])] public PaletteIndexBitmap Hyphen { get; private set; }
            [SpriteFrame("text_highlighted", [5, 4], [8, 8])] public PaletteIndexBitmap Colon { get; private set; }
        }
        public TextHighlightedSpriteStorage TextHighlighted { get; } = new TextHighlightedSpriteStorage();

        public sealed class TextPressedSpriteStorage : IHasSpritesToLoad
        {
            [SpriteFrame("text_pressed", [0, 0], [8, 8])] public PaletteIndexBitmap Zero { get; private set; }
            [SpriteFrame("text_pressed", [1, 0], [8, 8])] public PaletteIndexBitmap One { get; private set; }
            [SpriteFrame("text_pressed", [2, 0], [8, 8])] public PaletteIndexBitmap Two { get; private set; }
            [SpriteFrame("text_pressed", [3, 0], [8, 8])] public PaletteIndexBitmap Three { get; private set; }
            [SpriteFrame("text_pressed", [4, 0], [8, 8])] public PaletteIndexBitmap Four { get; private set; }
            [SpriteFrame("text_pressed", [5, 0], [8, 8])] public PaletteIndexBitmap Five { get; private set; }
            [SpriteFrame("text_pressed", [6, 0], [8, 8])] public PaletteIndexBitmap Six { get; private set; }
            [SpriteFrame("text_pressed", [7, 0], [8, 8])] public PaletteIndexBitmap Seven { get; private set; }
            [SpriteFrame("text_pressed", [8, 0], [8, 8])] public PaletteIndexBitmap Eight { get; private set; }
            [SpriteFrame("text_pressed", [9, 0], [8, 8])] public PaletteIndexBitmap Nine { get; private set; }

            [SpriteFrame("text_pressed", [0, 1], [8, 8])] public PaletteIndexBitmap A { get; private set; }
            [SpriteFrame("text_pressed", [1, 1], [8, 8])] public PaletteIndexBitmap B { get; private set; }
            [SpriteFrame("text_pressed", [2, 1], [8, 8])] public PaletteIndexBitmap C { get; private set; }
            [SpriteFrame("text_pressed", [3, 1], [8, 8])] public PaletteIndexBitmap D { get; private set; }
            [SpriteFrame("text_pressed", [4, 1], [8, 8])] public PaletteIndexBitmap E { get; private set; }
            [SpriteFrame("text_pressed", [5, 1], [8, 8])] public PaletteIndexBitmap F { get; private set; }
            [SpriteFrame("text_pressed", [6, 1], [8, 8])] public PaletteIndexBitmap G { get; private set; }
            [SpriteFrame("text_pressed", [7, 1], [8, 8])] public PaletteIndexBitmap H { get; private set; }
            [SpriteFrame("text_pressed", [8, 1], [8, 8])] public PaletteIndexBitmap I { get; private set; }
            [SpriteFrame("text_pressed", [9, 1], [8, 8])] public PaletteIndexBitmap J { get; private set; }

            [SpriteFrame("text_pressed", [0, 2], [8, 8])] public PaletteIndexBitmap K { get; private set; }
            [SpriteFrame("text_pressed", [1, 2], [8, 8])] public PaletteIndexBitmap L { get; private set; }
            [SpriteFrame("text_pressed", [2, 2], [8, 8])] public PaletteIndexBitmap M { get; private set; }
            [SpriteFrame("text_pressed", [3, 2], [8, 8])] public PaletteIndexBitmap N { get; private set; }
            [SpriteFrame("text_pressed", [4, 2], [8, 8])] public PaletteIndexBitmap O { get; private set; }
            [SpriteFrame("text_pressed", [5, 2], [8, 8])] public PaletteIndexBitmap P { get; private set; }
            [SpriteFrame("text_pressed", [6, 2], [8, 8])] public PaletteIndexBitmap Q { get; private set; }
            [SpriteFrame("text_pressed", [7, 2], [8, 8])] public PaletteIndexBitmap R { get; private set; }
            [SpriteFrame("text_pressed", [8, 2], [8, 8])] public PaletteIndexBitmap S { get; private set; }
            [SpriteFrame("text_pressed", [9, 2], [8, 8])] public PaletteIndexBitmap T { get; private set; }

            [SpriteFrame("text_pressed", [0, 3], [8, 8])] public PaletteIndexBitmap U { get; private set; }
            [SpriteFrame("text_pressed", [1, 3], [8, 8])] public PaletteIndexBitmap V { get; private set; }
            [SpriteFrame("text_pressed", [2, 3], [8, 8])] public PaletteIndexBitmap W { get; private set; }
            [SpriteFrame("text_pressed", [3, 3], [8, 8])] public PaletteIndexBitmap X { get; private set; }
            [SpriteFrame("text_pressed", [4, 3], [8, 8])] public PaletteIndexBitmap Y { get; private set; }
            [SpriteFrame("text_pressed", [5, 3], [8, 8])] public PaletteIndexBitmap Z { get; private set; }

            [SpriteFrame("text_pressed", [0, 4], [8, 8])] public PaletteIndexBitmap Comma { get; private set; }
            [SpriteFrame("text_pressed", [1, 4], [8, 8])] public PaletteIndexBitmap ExclamationMark { get; private set; }
            [SpriteFrame("text_pressed", [2, 4], [8, 8])] public PaletteIndexBitmap Period { get; private set; }
            [SpriteFrame("text_pressed", [3, 4], [8, 8])] public PaletteIndexBitmap QuestionMark { get; private set; }
            [SpriteFrame("text_pressed", [4, 4], [8, 8])] public PaletteIndexBitmap Hyphen { get; private set; }
            [SpriteFrame("text_pressed", [5, 4], [8, 8])] public PaletteIndexBitmap Colon { get; private set; }
        }
        public TextPressedSpriteStorage TextPressed { get; } = new TextPressedSpriteStorage();



        #region Loading
        public SpriteStorage()
        {
            Dictionary<string, PaletteIndexBitmap> bitmaps = new()
            {
                ["generic_background"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\generic_background.ptid"),
                ["player"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\player.ptid"),
                ["lantern"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\lantern.ptid"),
                ["enemies"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\enemies.ptid"),
                ["text_default"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\text_default.ptid"),
                ["text_highlighted"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\text_highlighted.ptid"),
                ["text_pressed"] = PaletteIndexBitmap.CreateFromFile($"resources\\sprites\\generated\\text_pressed.ptid"),
            };

            var frameAttributeType = typeof(SpriteFrameAttribute);
            var hasSpritesToLoadType = typeof(IHasSpritesToLoad);

            void LoadSprites(Type SpriteStorageType, object SpriteStorageObject)
            {
                foreach (var property in SpriteStorageType.GetProperties())
                {
                    if (property.PropertyType.GetInterfaces().Contains(hasSpritesToLoadType))
                    {
                        LoadSprites(property.GetValue(SpriteStorageObject)!.GetType(), property.GetValue(SpriteStorageObject)!);
                        continue;
                    }

                    var attribute = property.GetCustomAttribute(frameAttributeType);
                    if (attribute is not SpriteFrameAttribute frameAttribute)
                        continue;

                    var newBitmap = PaletteIndexBitmap.CreateEmpty(frameAttribute.Frame.Width, frameAttribute.Frame.Height);
                    newBitmap.Blit(bitmaps[frameAttribute.SpriteName], 0, 0, frameAttribute.Frame);
                    property.SetValue(SpriteStorageObject, newBitmap);
                }
            }

            LoadSprites(GetType(), this);

            foreach (var keyValuePair in bitmaps)
            {
                keyValuePair.Value.Dispose();
            }
        }

        public bool IsDisposed { get; private set; }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {

                }

                var frameAttributeType = typeof(SpriteFrameAttribute);
                var hasSpritesToLoadType = typeof(IHasSpritesToLoad);

                void UnloadSprites(Type SpriteStorageType, object SpriteStorageObject)
                {
                    foreach (var property in SpriteStorageType.GetProperties())
                    {
                        if (property.PropertyType.GetInterfaces().Contains(hasSpritesToLoadType))
                        {
                            UnloadSprites(property.GetValue(SpriteStorageObject)!.GetType(), property.GetValue(SpriteStorageObject)!);
                            continue;
                        }

                        var attribute = property.GetCustomAttribute(frameAttributeType);
                        if (attribute is not SpriteFrameAttribute frameAttribute)
                            continue;

                        (property.GetValue(SpriteStorageObject) as PaletteIndexBitmap)!.Dispose();
                    }
                }

                UnloadSprites(GetType(), this);

                IsDisposed = true;
            }
        }

        ~SpriteStorage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    public SpriteStorage Sprites { get; }

    public sealed class AudioStorage
    {
        public AudioStorage()
        {
            SDL.LoadWAV($"resources\\sounds\\hurt1.wav", out var audioSpec, out var audioBuffer, out var audioLength);
        }
    }
    public AudioStorage Audio { get; }

    public AssetStorage()
    {
        Sprites = new SpriteStorage();
        Audio = new AudioStorage();
    }
}


#pragma warning restore CS8618