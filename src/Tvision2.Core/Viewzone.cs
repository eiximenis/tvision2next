namespace Tvision2.Core;

public readonly record struct Viewzone(TvPoint TopLeft, TvBounds Bounds)
{
    public static Viewzone FromTopLeftToBottomRight(TvPoint topLeft, TvPoint bottomRight) =>
        new Viewzone(topLeft, TvPoint.CalculateBounds(topLeft, bottomRight));
    
    public TvPoint BottomRight => TopLeft + Bounds;
    public static Viewzone FromBounds(TvBounds bounds) => new Viewzone(TvPoint.Zero, bounds);
}