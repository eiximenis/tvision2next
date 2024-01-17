using Tvision2.Core;
using Tvision2.Engine.Components;

namespace Tvision2.Engine.Layout;

public static class LayoutManagers
{
    public static readonly ILayoutManager Absolute = new AbsoluteLayoutManager();
    public static ILayoutManager FixedPosition(TvPoint position) => new FixedLayoutManager(position);

    public static ILayoutManager Blocked(TvPoint position, TvBounds bounds) =>
        new BlockedLayoutManager(position, bounds);
}

public class BlockedLayoutManager(TvPoint position, TvBounds bounds) : ILayoutManager
{
    public void UpdateLayout(TvComponentMetadata metadata)
    {
        var cmp = metadata.Component;
        if (cmp.Viewport.Position != position)
        {
            cmp.Viewport.MoveTo(position);
        }

        if (cmp.Viewport.Bounds != bounds)
        {
            cmp.Viewport.Resize(bounds);
        }
    }
}

class FixedLayoutManager(TvPoint position) : ILayoutManager
{
    public void UpdateLayout(TvComponentMetadata metadata)
    {
        var cmp = metadata.Component;
        if (cmp.Viewport.Position != position)
        {
            cmp.Viewport.MoveTo(position);
        }
    }
}

class AbsoluteLayoutManager : ILayoutManager
{
    public void UpdateLayout(TvComponentMetadata metadata)
    {
        // Absolute position is a no-op because nothing needs to be calculated.
    }
    
}
