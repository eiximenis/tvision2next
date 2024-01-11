namespace Tvision2.Core;


    
[Flags]
public enum ViewportUpdateReason
{
    Moved = 0x1,
    Resized = 0x2
}


static class ViewportUpdateReasonExtensions
{
    public static bool HasMoved(this ViewportUpdateReason reason) =>
        (reason & ViewportUpdateReason.Moved) == ViewportUpdateReason.Moved;

    public static bool HasResized(this ViewportUpdateReason reason) =>
        (reason & ViewportUpdateReason.Resized) == ViewportUpdateReason.Resized;
}

public class Viewport
{
    private readonly Viewzone _viewzone;
    public TvPoint Position { get; }
    public TvBounds Bounds { get;  }
    public static Viewport FullViewport { get; } = new Viewport(TvPoint.Zero, TvBounds.ConsoleBounds);
    public static Viewport Null() => new Viewport(TvPoint.Zero, TvBounds.Empty);
    public bool IsNull => Bounds.IsEmpty;

    public Viewport(TvPoint position, TvBounds bounds)
    {
        Position = position;
        Bounds = bounds;
        _viewzone = new Viewzone(Position, Bounds);
    }

    public Viewzone Viewzone => _viewzone;
}