using Tvision2.Core;

namespace Tvision2.Engine.Components;
    
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
    private Viewzone _viewzone;
    public TvPoint Position { get; private set; }
    public TvBounds Bounds { get; private set; }
    public static Viewport FullViewport { get; } = new Viewport(TvPoint.Zero, TvBounds.ConsoleBounds);
    public static Viewport Null() => new Viewport(TvPoint.Zero, TvBounds.Empty);
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

    public void MoveTo(TvPoint newPos)
    {
        Position = newPos;
        HasLayoutPending = true;
    }

    public void Resize(TvBounds newBounds)
    {
        Bounds = newBounds;
        HasLayoutPending = true;
    }

    internal void LayoutUpdated()
    {
        _viewzone = new Viewzone(Position, Bounds);
        HasLayoutPending = false;
    }
}