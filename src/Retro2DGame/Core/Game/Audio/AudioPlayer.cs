using Retro2DGame.Core.MixerWrappers;

namespace Retro2DGame.Core.Game.Audio;

internal sealed class AudioPlayer : IDisposable
{
    public sealed class IndependentAudioHandle
    {
        private readonly MixerTrack _track;

        public float Volume
        {
            get
            {
                return _track.GetGain();
            }
            set
            {
                _track.SetGain(value);
            }
        }

        public IndependentAudioHandle(MixerTrack track)
        {
            _track = track;
        }

        public bool IsFinished()
        {
            return _track.GetRemainingSampleFrames() <= 0;
        }

        public void Stop()
        {
            _track.Stop();
        }
    }

    private readonly Mixer _mixer;

    private MixerTrack[] _tracks;

    private readonly int _trackAmountToAddWhenFull;

    public bool IsDisposed { get; private set; }

    public float GlobalVolume
    {
        get
        {
            return _mixer.GetGain();
        }
        set
        {
            _mixer.SetGain(value);
        }
    }

    public AudioPlayer(int initialTrackCount)
    {
        _mixer = Mixer.Create(SDL3.SDL.AudioDeviceDefaultPlayback, nint.Zero);

        _tracks = new MixerTrack[initialTrackCount];
        for (int i = 0; i < _tracks.Length; i++)
        {
            _tracks[i] = MixerTrack.Create(_mixer);
        }

        _trackAmountToAddWhenFull = initialTrackCount;
    }

    public IndependentAudioHandle? Play(AudioEffect audio, AudioPlayParameters parameters)
    {
        void ActuallyPlay(long trackId)
        {
            var track = _tracks[trackId];

            track.SetAudio(audio.Audio);

            track.SetGain(parameters.Volume);
            track.SetFrequencyRatio(parameters.Pitch);

            uint optionProperties = SDL3.SDL.CreateProperties();
            if (parameters.ShouldLoop)
            {
                SDL3.SDL.SetNumberProperty(optionProperties, SDL3.Mixer.Props.PlayLoopsNumber, -1);
            }

            track.Play(optionProperties);
        }

        for (int i = 0; i < _tracks.Length; i++)
        {
            if (_tracks[i].GetRemainingSampleFrames() <= 0)
            {
                ActuallyPlay(i);
                return new IndependentAudioHandle(_tracks[i]);
            }
        }

        if (!parameters.IsImportant)
            return null;

        var indexToPlay = _tracks.Length;
        Array.Resize(ref _tracks, _tracks.Length + _trackAmountToAddWhenFull);

        ActuallyPlay(indexToPlay);
        return new IndependentAudioHandle(_tracks[indexToPlay]);
    }

    public void PausePlayback()
    {
        _mixer.Lock();
    }

    public void ResumePlayback()
    {
        _mixer.Unlock();
    }

    public void PauseAudio()
    {
        _mixer.PauseAllTracks();
    }

    public void ResumeAudio()
    {
        _mixer.ResumeAllTracks();
    }

    public void StopAudio()
    {
        _mixer.StopAllTracks();
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                _mixer.Dispose();
                for (int i = 0; i < _tracks.Length; i++)
                {
                    _tracks[i].Dispose();
                }
            }

            IsDisposed = true;
        }
    }

    ~AudioPlayer()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
