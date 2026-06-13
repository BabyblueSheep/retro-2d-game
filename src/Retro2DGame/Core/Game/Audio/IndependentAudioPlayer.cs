using Retro2DGame.Core.MixerWrappers;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace Retro2DGame.Core.Game.Audio;

internal sealed class IndependentAudioPlayer : IDisposable
{
    private readonly Mixer _mixer;

    private readonly MixerTrack[] _tracks;

    public bool IsDisposed { get; private set; }

    private float _globalVolume;
    public float GlobalVolume
    {
        get
        {
            return _globalVolume;
        }
        set
        {
            _globalVolume = value;
            _mixer.SetGain(_globalVolume);
        }
    }

    public IndependentAudioPlayer(int initialTrackCount)
    {
        _mixer = Mixer.Create(SDL3.SDL.AudioDeviceDefaultPlayback, nint.Zero);

        _tracks = new MixerTrack[initialTrackCount];
        for (int i = 0; i < _tracks.Length; i++)
        {
            _tracks[i] = MixerTrack.Create(_mixer);
        }
    }

    public void Play(AudioEffect audio, AudioPlayParameters parameters)
    {
        void ActuallyPlay(long trackId)
        {
            var track = _tracks[trackId];

            track.SetAudio(audio.Audio);

            track.SetGain(parameters.Volume);
            track.SetFrequencyRatio(parameters.Pitch);

            track.Play();
        }

        if (_tracks[0].GetRemainingSampleFrames() <= 0)
        {
            ActuallyPlay(0);
            return;
        }    

        long indexWithSmallestRemainingFrames = 0;
        for (int i = 1; i < _tracks.Length; i++)
        {
            if (_tracks[i].GetRemainingSampleFrames() <= 0)
            {
                ActuallyPlay(i);
                return;
            }

            if (_tracks[indexWithSmallestRemainingFrames].GetRemainingSampleFrames() > _tracks[i].GetRemainingSampleFrames())
            {
                indexWithSmallestRemainingFrames = i;
            }
        }

        ActuallyPlay(indexWithSmallestRemainingFrames);
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

    ~IndependentAudioPlayer()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
