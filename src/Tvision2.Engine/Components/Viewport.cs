using Tvision2.Core;

namespace Tvision2.Engine.Components;
    
[Flags]
public enum ViewportUpdateReason
{
    None = 0x0,
    Moved = 0x1,
    Resized = 0x2,
    MovedAndResizes = Moved | Resized,
    All = Moved | Resized
}


static class ViewportUpdateReasonExtensions
{
    public static bool HasMoved(this ViewportUpdateReason reason) =>
        (reason & ViewportUpdateReason.Moved) == ViewportUpdateReason.Moved;

    public static bool HasResized(this ViewportUpdateReason reason) =>
        (reason & ViewportUpdateReason.Resized) == ViewportUpdateReason.Resized;
}


public static class Viewports
{
    public static Viewport FullViewport { get; } = new Viewport(TvPoint.Zero, TvBounds.ConsoleBounds);
    public static Viewport Null() => new Viewport(TvPoint.Zero, TvBounds.Empty);
}

public class Viewport
{
    private Viewzone _viewzone;
    public TvPoint Position { get; private set; }
    public TvBounds Bounds { get; private set; }
    public TvPoint BottomRight => Position + Bounds;
    

    public bool IsNull => Bounds.IsEmpty;
    
    public bool HasLayoutPending { get; private set; }

    public Viewport(TvPoint position, TvBounds bounds)
    {
        Position = position;
        Bounds = bounds;
        _viewzone = new Viewzone(Position, Bounds);
        HasLayoutPending = true;
    }

    public Viewzone Viewzone => _viewzone;

    public ViewportUpdateReason MoveTo(TvPoint newPos)
    {
        if (Position == newPos) return ViewportUpdateReason.None;
        
        Position = newPos;
        HasLayoutPending = true;
        return ViewportUpdateReason.Moved;
    }

    public ViewportUpdateReason Resize(TvBounds newBounds)
    {
        if (Bounds == newBounds) return ViewportUpdateReason.None;
        
        Bounds = newBounds;
        HasLayoutPending = true;
        return ViewportUpdateReason.Resized;
    }

    internal void LayoutUpdated()
    {
        _viewzone = new Viewzone(Position, Bounds);
        HasLayoutPending = false;
    }
}