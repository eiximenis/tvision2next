using Tvision2.Core;

namespace Tvision2.Engine.Components;

[Flags]
public enum ViewportUpdateReason
{
    None = 0x0,
    Moved = 0x1,
    Resized = 0x2,
    MovedAndResized = Moved | Resized,
    All = Moved | Resized
}


public static class ViewportUpdateReasonExtensions
{
    public static bool HasMoved(this ViewportUpdateReason reason) =>
        (reason & ViewportUpdateReason.Moved) == ViewportUpdateReason.Moved;

    public static bool HasResized(this ViewportUpdateReason reason) =>
        (reason & ViewportUpdateReason.Resized) == ViewportUpdateReason.Resized;

    public static bool Any(this ViewportUpdateReason reason) => reason != ViewportUpdateReason.None;
}


public static class Viewports
{
    public static Viewport FullViewport { get; } = new Viewport(TvPoint.Zero, TvBounds.ConsoleBounds);
    public static Viewport Null() => new Viewport(TvPoint.Zero, TvBounds.Empty);
}

public interface IViewportSnapshot
{
    TvPoint Position { get; }
    TvBounds Bounds { get; }
    TvPoint BottomRight => Position + Bounds;
    bool IsNull => Bounds.IsEmpty;
}

public readonly record struct ViewportSnapshot(TvPoint Position, TvBounds Bounds) : IViewportSnapshot
{
    public TvPoint BottomRight => Position + Bounds;
    public bool IsNull => Bounds.IsEmpty;
}

public class Viewport : IViewportSnapshot
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

    public ViewportUpdateReason MoveAndResize(TvPoint newPos, TvBounds newBounds)
    {
        var moved = MoveTo(newPos);
        var resized = Resize(newBounds);
        return moved | resized;
    }

    internal void LayoutUpdated()
    {
        _viewzone = new Viewzone(Position, Bounds);
        HasLayoutPending = false;
    }
}

public static class ViewportSnapshots
{
    public static IViewportSnapshot WithMargin(Viewport viewport, Margin margin)
    {
        var newPos = viewport.Position + TvPoint.FromXY(margin.Left, margin.Top);
        var newRows = viewport.Bounds.Height - margin.Top - margin.Bottom;
        var newCols = viewport.Bounds.Width - margin.Left - margin.Right;
        var newBounds = TvBounds.FromRowsAndCols(newRows > 0 ? newRows : 0, newCols > 0 ? newCols : 0);
        return new ViewportSnapshot(newPos, newBounds);
    }
}