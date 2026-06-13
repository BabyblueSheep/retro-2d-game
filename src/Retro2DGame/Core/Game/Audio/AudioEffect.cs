using Retro2DGame.Core.MixerWrappers;
using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Audio;

internal sealed class AudioEffect : IDisposable
{
    public MixerAudio Audio { get; }

    public bool IsDisposed { get; private set; }

    private AudioEffect(MixerAudio audio)
    {
        Audio = audio;
    }

    public static AudioEffect Load(string path)
    {
        var mixerAudio = MixerAudio.Load(nint.Zero, path, true);
        var audioEffect = new AudioEffect(mixerAudio);
        return audioEffect;
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                Audio.Dispose();
            }

            IsDisposed = true;
        }
    }

    ~AudioEffect()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
