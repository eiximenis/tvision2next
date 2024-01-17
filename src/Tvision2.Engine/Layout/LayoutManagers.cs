using Tvision2.Core;
using Tvision2.Engine.Components;

namespace Tvision2.Engine.Layout;

public static class LayoutManagers
{
    public static readonly ILayoutManager Absolute = new AbsoluteLayoutManager();
    public static ILayoutManager Fixed(TvPoint position) => new FixedLayoutManager(position);
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
