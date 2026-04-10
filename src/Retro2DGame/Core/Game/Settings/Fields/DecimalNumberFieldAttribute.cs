using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Core.Game.Settings.Fields;

[AttributeUsage (AttributeTargets.Field | AttributeTargets.Property)]
internal sealed class DecimalNumberFieldAttribute : Attribute
{
    public double Start { get; set; }
    public double End { get; set; }
    public uint DecimalDigitAmount { get; set; }

    public DecimalNumberFieldAttribute()
    {
        Start = 0.0;
        End = 1.0;
        DecimalDigitAmount = 3;
    }
}
