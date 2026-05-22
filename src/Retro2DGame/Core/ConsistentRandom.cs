using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core;

// Implementation is based on xoshiro256++, with SplitMix64 for seed generation.

internal sealed class ConsistentRandom
{
    private static ulong NextSplitMix64(ulong seed)
    {
        ulong z = (seed += 0x9e3779b97f4a7c15);
        z = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
        z = (z ^ (z >> 27)) * 0x94d049bb133111eb;
        return z ^ (z >> 31);
    }

    public ConsistentRandom(ulong seed)
    {
        _seed0 = NextSplitMix64(seed);
        _seed1 = NextSplitMix64(_seed0);
        _seed2 = NextSplitMix64(_seed1);
        _seed3 = NextSplitMix64(_seed2);
    }

    public ConsistentRandom() : this(1)
    {

    }

    private ulong _seed0;
    private ulong _seed1;
    private ulong _seed2;
    private ulong _seed3;

    private static ulong RotateLeft(ulong x, int k)
    {
        return (x << k) | (x >> (64 - k));
    }

    public ulong Next()
    {
        ulong result = RotateLeft(_seed0 + _seed3, 23) + _seed0;

        ulong t = _seed1 << 17;

        _seed2 ^= _seed0;
        _seed3 ^= _seed1;
        _seed1 ^= _seed2;
        _seed0 ^= _seed3;

        _seed2 ^= t;

        _seed3 = RotateLeft(_seed3, 45);

        return result;
    }



    public int RandomInt()
    {
        return (int)(Next() >> 33);
    }

    public int RandomInt(int max)
    {
        return RandomInt() % max;
    }

    public int RandomInt(int min, int max)
    {
        return min + RandomInt(max - min);
    }

    public float RandomFloat()
    {
        return Next() / ((float)ulong.MaxValue + 1f);
    }

    public float RandomFloat(float max)
    {
        return RandomFloat() * max;
    }

    public float RandomFloat(float min, float max)
    {
        return float.Lerp(min, max, RandomFloat());
    }
}
