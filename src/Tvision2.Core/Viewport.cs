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
    public TvPoint Position { get; private set; }
    public TvBounds Bounds { get; private set; }
    public static Viewport Null() => new Viewport() { Position = TvPoint.Zero, Bounds = TvBounds.Empty };
    
    
    
}