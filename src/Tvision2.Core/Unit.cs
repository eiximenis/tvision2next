using System.Diagnostics.CodeAnalysis;

namespace Tvision2.Core;

public readonly struct Unit
{
    private static Unit _value = new Unit();

    public static Unit Value => _value;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Unit;
    }

    public bool Equals(Unit _) => true;

    public static bool operator ==(Unit _1, Unit _2) => true;

    public static bool operator !=(Unit _1, Unit _2) => false;
        
    public override int GetHashCode() => HashCode.Combine(0, 1);
}