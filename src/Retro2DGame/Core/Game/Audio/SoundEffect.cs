using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game.Audio;

internal sealed class SoundEffect : IDisposable
{
    public nint Handle { get; }
    public uint Length { get; }

    public SDL.AudioSpec Spec { get; }

    public bool IsDisposed { get; private set; }

    private SoundEffect(nint bufferHandle, uint length, SDL.AudioSpec spec)
    {
        Handle = bufferHandle;
        Length = length;
        Spec = spec;
    }

    public static SoundEffect LoadWAV(string path)
    {
        var result = SDL.LoadWAV(path, out var audioSpec, out var audioBuffer, out var audioLength);
        if (!result)
        {
            throw new Exception($"Couldn't create renderer: {SDL.GetError()}");
        }

        var soundEffect = new SoundEffect(audioBuffer, audioLength, audioSpec);

        return soundEffect;
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                
            }

            SDL.Free(Handle);

            IsDisposed = true;
        }
    }

    ~SoundEffect()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
