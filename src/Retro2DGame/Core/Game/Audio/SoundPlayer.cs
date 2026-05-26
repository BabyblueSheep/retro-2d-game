using Retro2DGame.Core.SDL3;
using SDL3;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Retro2DGame.Core.Game.Audio;

internal sealed class SoundPlayer : IDisposable
{
    private readonly AudioStream _stream;

    public bool IsDisposed { get; private set; }

    public SoundPlayer()
    {
        _stream = AudioStream.OpenStream(nint.Zero);
        _stream.Resume();
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            _stream.Dispose();

            IsDisposed = true;
        }
    }

    ~SoundPlayer()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
