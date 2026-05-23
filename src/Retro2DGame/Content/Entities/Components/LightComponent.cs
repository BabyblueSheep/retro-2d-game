using Frent;
using Frent.Systems;

namespace Retro2DGame.Content.Entities.Components;

internal record struct Light(float InnerRadius, float OuterRadius);

internal record struct LightOscillator(TimeSpan Timer, float BaseInner, float OffsetInner, float SpeedInner, float PhaseInner, float BaseOuter, float OffsetOuter, float SpeedOuter, float PhaseOuter)
{
    public static void UpdateLightOscillators(World world, TimeSpan delta)
    {
        var lightOscillators = world
            .CreateQuery()
            .With<Light>()
            .With<LightOscillator>()
            .Build();

        lightOscillators.Delegate((ref Light light, ref LightOscillator lightOscillator) =>
        {
            lightOscillator.Timer += delta;

            light.InnerRadius = float.Lerp
            (
                lightOscillator.BaseInner - lightOscillator.OffsetInner,
                lightOscillator.BaseInner + lightOscillator.OffsetInner,
                float.Sin((float)lightOscillator.Timer.TotalSeconds * lightOscillator.SpeedInner + lightOscillator.PhaseInner) * 0.5f + 0.5f
            );

            light.OuterRadius = float.Lerp
            (
                lightOscillator.BaseOuter - lightOscillator.OffsetOuter,
                lightOscillator.BaseOuter + lightOscillator.OffsetOuter,
                float.Sin((float)lightOscillator.Timer.TotalSeconds * lightOscillator.SpeedOuter + lightOscillator.PhaseOuter) * 0.5f + 0.5f
            );
        });
    }
}