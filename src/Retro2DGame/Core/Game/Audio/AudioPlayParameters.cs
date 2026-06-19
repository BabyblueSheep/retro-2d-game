namespace Retro2DGame.Core.Game.Audio;

public struct AudioPlayParameters
{
    public AudioPlayParameters() { }

    public float Volume { get; set; } = 1f;
    public float Pitch { get; set; } = 1f;

    public bool ShouldLoop { get; set; } = false;
    public bool IsImportant { get; set; } = false;
}