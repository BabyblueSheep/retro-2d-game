using SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Game.Core.SDL3.Rendering.Structures;

public interface IVertexType
{
    static abstract SDL.GPUVertexElementFormat[] Formats { get; }

    static abstract uint[] Offsets { get; }
    
    static abstract uint Size { get; }
}

[StructLayout(LayoutKind.Explicit, Size = 32)]
internal struct PositionColorVertex : IVertexType
{
    [FieldOffset(0)]
    public float X;

    [FieldOffset(4)]
    public float Y;

    [FieldOffset(8)]
    public float Z;

    [FieldOffset(16)]
    public byte R;

    [FieldOffset(20)]
    public byte G;

    [FieldOffset(24)]
    public byte B;

    [FieldOffset(28)]
    public byte A;

    public static SDL.GPUVertexElementFormat[] Formats =>
    [
        SDL.GPUVertexElementFormat.Float3,
        SDL.GPUVertexElementFormat.Ubyte4Norm,
    ];

    public static uint[] Offsets =>
    [
        0,
        16,
    ];

    public static uint Size => 32;
}
