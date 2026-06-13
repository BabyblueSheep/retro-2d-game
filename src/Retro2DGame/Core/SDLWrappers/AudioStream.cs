namespace Retro2DGame.Core.SDLWrappers;

internal sealed class AudioStream : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private AudioStream(nint handle)
    {
        Handle = handle;
    }

    public static AudioStream OpenStream(nint spec)
    {
        var handle = SDL3.SDL.OpenAudioDeviceStream(SDL3.SDL.AudioDeviceDefaultPlayback, spec, null, nint.Zero);

        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create audio stream: {SDL3.SDL.GetError()}");
        }

        var stream = new AudioStream(handle);
        return stream;
    }

    public static AudioStream OpenStream(in SDL3.SDL.AudioSpec spec)
    {
        var handle = SDL3.SDL.OpenAudioDeviceStream(SDL3.SDL.AudioDeviceDefaultPlayback, spec, null, nint.Zero);

        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create audio stream: {SDL3.SDL.GetError()}");
        }

        var stream = new AudioStream(handle);
        return stream;
    }

    public bool Clear()
    {
        return SDL3.SDL.ClearAudioStream(Handle);
    }

    public int GetQueuedBytes()
    {
        return SDL3.SDL.GetAudioStreamQueued(Handle);
    }

    public bool PutData(nint buffer, int length)
    {
        return SDL3.SDL.PutAudioStreamData(Handle, buffer, length);
    }

    public bool PutData(byte[] buffer, int length)
    {
        return SDL3.SDL.PutAudioStreamData(Handle, buffer, length);
    }

    public bool Resume()
    {
        return SDL3.SDL.ResumeAudioStreamDevice(Handle);
    }

    public bool GetFormat(out SDL3.SDL.AudioSpec sourceSpec, out SDL3.SDL.AudioSpec destinationSpec)
    {
        return SDL3.SDL.GetAudioStreamFormat(Handle, out sourceSpec, out destinationSpec);
    }

    public bool SetFormat(in SDL3.SDL.AudioSpec sourceSpec, in SDL3.SDL.AudioSpec destinationSpec)
    {
        return SDL3.SDL.SetAudioStreamFormat(Handle, sourceSpec, destinationSpec);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL3.SDL.DestroyAudioStream(Handle);

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