namespace Retro2DGame.Core.MixerWrappers;

internal sealed class Mixer : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private Mixer(nint handle)
    {
        Handle = handle;
    }

    public static Mixer Create(uint deviceId, nint spec)
    {
        var handle = SDL3.Mixer.CreateMixerDevice(deviceId, spec);

        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create mixer: {SDL3.SDL.GetError()}");
        }

        var mixer = new Mixer(handle);
        return mixer;
    }

    public bool SetFrequencyRatio(float frequencyRatio)
    {
        return SDL3.Mixer.SetMixerFrequencyRatio(Handle, frequencyRatio);
    }

    public bool SetGain(float gain)
    {
        return SDL3.Mixer.SetMixerGain(Handle, gain);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL3.Mixer.DestroyMixer(Handle);

            IsDisposed = true;
        }
    }

    ~Mixer()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
