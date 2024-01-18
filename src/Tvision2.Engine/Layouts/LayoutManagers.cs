using Tvision2.Core;
using Tvision2.Engine.Components;

namespace Tvision2.Engine.Layouts;

public static class LayoutManagers
{
    public static readonly ILayoutManager Absolute = new AbsoluteLayoutManager();
    public static ILayoutManager FixedPosition(TvPoint position) => new FixedLayoutManager(position);

    public static ILayoutManager Blocked(TvPoint position, TvBounds bounds) =>
        new BlockedLayoutManager(position, bounds);
}

public class BlockedLayoutManager(TvPoint position, TvBounds bounds) : ILayoutManager
{
    public ViewportUpdateReason UpdateLayout(TvComponentMetadata metadata)
    {
        var cmp = metadata.Component;
        var updated = ViewportUpdateReason.None;
        if (cmp.Viewport.Position != position)
        {
            updated |= cmp.Viewport.MoveTo(position);
             
        }

        if (cmp.Viewport.Bounds != bounds)
        {
            updated |= cmp.Viewport.Resize(bounds);
        }
        
        return updated;
    }
}

class FixedLayoutManager(TvPoint position) : ILayoutManager
{
    public ViewportUpdateReason UpdateLayout(TvComponentMetadata metadata)
    {
        var cmp = metadata.Component;
        if (cmp.Viewport.Position != position)
        {
            return cmp.Viewport.MoveTo(position);
        }

        return ViewportUpdateReason.None;
    }
}

class AbsoluteLayoutManager : ILayoutManager
{
    public ViewportUpdateReason UpdateLayout(TvComponentMetadata metadata)
    {
        // Absolute position is a no-op because nothing needs to be calculated.
        return ViewportUpdateReason.All;
    }
    
}
