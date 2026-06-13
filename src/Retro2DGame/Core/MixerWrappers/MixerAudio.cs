namespace Retro2DGame.Core.MixerWrappers;

internal sealed class MixerAudio : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private MixerAudio(nint handle)
    {
        Handle = handle;
    }

    public static MixerAudio Load(nint mixer, string path, bool preDecode)
    {
        var handle = SDL3.Mixer.LoadAudio(mixer, path, preDecode);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create mixer audio: {SDL3.SDL.GetError()}");
        }

        var audio = new MixerAudio(handle);
        return audio;
    }

    public static MixerAudio Load(Mixer mixer, string path, bool preDecode)
    {
        var handle = SDL3.Mixer.LoadAudio(mixer.Handle, path, preDecode);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create mixer audio: {SDL3.SDL.GetError()}");
        }

        var audio = new MixerAudio(handle);
        return audio;
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL3.Mixer.DestroyAudio(Handle);

            IsDisposed = true;
        }
    }

    ~MixerAudio()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
