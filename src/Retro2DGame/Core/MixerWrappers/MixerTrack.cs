namespace Retro2DGame.Core.MixerWrappers;

internal sealed class MixerTrack : IDisposable
{
    public nint Handle { get; }

    public bool IsDisposed { get; private set; }

    private MixerTrack(nint handle)
    {
        Handle = handle;
    }

    public static MixerTrack Create(Mixer mixer)
    {
        var handle = SDL3.Mixer.CreateTrack(mixer.Handle);
        if (handle == nint.Zero)
        {
            throw new Exception($"Couldn't create mixer track: {SDL3.SDL.GetError()}");
        }

        var track = new MixerTrack(handle);
        return track;
    }

    public long GetRemainingSampleFrames()
    {
        return SDL3.Mixer.GetTrackRemaining(Handle);
    }

    public bool SetAudio(MixerAudio audio)
    {
        return SDL3.Mixer.SetTrackAudio(Handle, audio.Handle);
    }

    public bool SetFrequencyRatio(float frequencyRatio)
    {
        return SDL3.Mixer.SetTrackFrequencyRatio(Handle, frequencyRatio);
    }

    public float GetGain()
    {
        return SDL3.Mixer.GetTrackGain(Handle);
    }

    public bool SetGain(float gain)
    {
        return SDL3.Mixer.SetTrackGain(Handle, gain);
    }

    public bool Pause()
    {
        return SDL3.Mixer.PauseTrack(Handle);
    }

    public bool Play(uint options)
    {
        return SDL3.Mixer.PlayTrack(Handle, options);
    }

    public bool Resume()
    {
        return SDL3.Mixer.ResumeTrack(Handle);
    }

    public bool Stop()
    {
        return SDL3.Mixer.StopTrack(Handle, 0);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {

            }

            SDL3.Mixer.DestroyTrack(Handle);

            IsDisposed = true;
        }
    }

    ~MixerTrack()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
