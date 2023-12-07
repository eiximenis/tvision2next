using System.Diagnostics.CodeAnalysis;

namespace Tvision2.Core;

public readonly struct TvBounds : IEquatable<TvBounds>
{
    public int Width { get; }
    public int Height { get; }
        
    public int Length => Height * Width;

    private TvBounds(int rows, int cols)
    {
        Height = rows >= 0 ? rows : 0;
        Width = cols >= 0 ? cols : 0;
    }
    public static TvBounds Empty => new TvBounds(0, 0);

    public bool IsEmpty => Height == 0 || Width == 0;
    public static TvBounds Console => new TvBounds(System.Console.WindowHeight, System.Console.WindowWidth);

    public static TvBounds FromRowsAndCols(int rows, int cols) => new TvBounds(rows, cols);

    public bool Equals(TvBounds other) => Height == other.Height && Width == other.Width;
        
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj switch
        {
            TvBounds other => Equals(other),
            _ => false
        };
    }
        
    public void Deconstruct(out int width, out int height)
    {
        width = Width;
        height = Height;
    }

    public static bool operator ==(TvBounds one, TvBounds two) => one.Equals(two);
    public static bool operator !=(TvBounds one, TvBounds two) => !one.Equals(two);


    public override int GetHashCode() => HashCode.Combine(Height, Width);

    public override string ToString() => $"<{Height}h,{Width}w>";
}