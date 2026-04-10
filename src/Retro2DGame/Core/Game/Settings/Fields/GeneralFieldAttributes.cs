using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Core.Game.Settings.Fields;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
internal sealed class PositionAttribute : Attribute
{
    public int Position { get; set; }

    public PositionAttribute(int position)
    {
        Position = position;
    }
}