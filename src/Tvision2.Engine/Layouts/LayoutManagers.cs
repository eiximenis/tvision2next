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
    public ViewportUpdateReason UpdateLayout(Viewport viewportToUpdate)
    {
        var updated = ViewportUpdateReason.None;
        if (viewportToUpdate.Position != position)
        {
            updated |= viewportToUpdate.MoveTo(position);
             
        }

        if (viewportToUpdate.Bounds != bounds)
        {
            updated |= viewportToUpdate.Resize(bounds);
        }
        
        return updated;
    }
}

class FixedLayoutManager(TvPoint position) : ILayoutManager
{
    public ViewportUpdateReason UpdateLayout(Viewport viewportToUpdate)
    {
        if (viewportToUpdate.Position != position)
        {
            return viewportToUpdate.MoveTo(position);
        }

        return ViewportUpdateReason.None;
    }
}

class AbsoluteLayoutManager : ILayoutManager
{
    public ViewportUpdateReason UpdateLayout(Viewport viewportToUpdate)
    {
        // Absolute position is a no-op because nothing needs to be calculated.
        return ViewportUpdateReason.All;
    }
    
}
