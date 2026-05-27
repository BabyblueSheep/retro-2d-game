using SDL3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.SDL3;

internal class AudioStream : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private AudioStream(nint handle)
    {
        Handle = handle;
    }

    public static AudioStream OpenStream(nint spec)
    {
        var handle = SDL.OpenAudioDeviceStream(SDL.AudioDeviceDefaultPlayback, spec, null, nint.Zero);

        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create audio stream: {SDL.GetError()}");
        }

        var stream = new AudioStream(handle);
        return stream;
    }

    public static AudioStream OpenStream(in SDL.AudioSpec spec)
    {
        var handle = SDL.OpenAudioDeviceStream(SDL.AudioDeviceDefaultPlayback, spec, null, nint.Zero);

        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create audio stream: {SDL.GetError()}");
        }

        var stream = new AudioStream(handle);
        return stream;
    }

    public bool Clear()
    {
        return SDL.ClearAudioStream(Handle);
    }

    public int GetQueuedBytes()
    {
        return SDL.GetAudioStreamQueued(Handle);
    }

    public bool PutData(nint buffer, int length)
    {
        return SDL.PutAudioStreamData(Handle, buffer, length);
    }

    public bool PutData(byte[] buffer, int length)
    {
        return SDL.PutAudioStreamData(Handle, buffer, length);
    }

    public bool Resume()
    {
        return SDL.ResumeAudioStreamDevice(Handle);
    }

    public bool GetFormat(out SDL.AudioSpec sourceSpec, out SDL.AudioSpec destinationSpec)
    {
        return SDL.GetAudioStreamFormat(Handle, out sourceSpec, out destinationSpec);
    }

    public bool SetFormat(in SDL.AudioSpec sourceSpec, in SDL.AudioSpec destinationSpec)
    {
        return SDL.SetAudioStreamFormat(Handle, sourceSpec, destinationSpec);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL.DestroyAudioStream(Handle);

            IsDisposed = true;
        }
    }

    ~AudioStream()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}