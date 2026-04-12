using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Retro2DGame.Core.SDL3;

// https://github.com/MoonsideGames/MoonWorks/blob/main/src/InteropUtilities.cs
internal static class PointerHelper
{
    public static unsafe byte* EncodeToUTF8Buffer(this string s, out int length)
    {
        if (s == null)
        {
            length = 0;
            return null;
        }

        length = Encoding.UTF8.GetByteCount(s) + 1;
        var buffer = (byte*)NativeMemory.Alloc((nuint)length);
        var span = new Span<byte>(buffer, length - 1);
        var byteCount = Encoding.UTF8.GetBytes(s, span);
        buffer[byteCount] = 0;

        return buffer;
    }
}
