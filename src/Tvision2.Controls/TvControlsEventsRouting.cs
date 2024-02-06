using Tvision2.Console.Events;
using Tvision2.Engine;

namespace Tvision2.Controls;

/// <summary>
/// This class "routes" events to focused controls.
/// It will tunnel the events first (calling PreviewEvents) of specified controls
/// and bubble the events later (calling HandleEvents).
///
/// Not all events support bubbling/tunneling
/// </summary>
public class TvControlsEventsRouting : IHook
{
    private readonly TvControlsOptions _options;
    private readonly TvControlsTree _tree;
    
    public TvControlsEventsRouting(TvControlsOptions options, TvControlsTree controlsTree)
    {
        _options = options;
        _tree = controlsTree;
    }
    
    public async Task BeforeUpdate(TvConsoleEvents events)
    {
        if (events.Count > 0)
        {
            foreach (var cmetadata in _tree.TunnelingControls)
            {
                await cmetadata.Control.PreviewEvents(events);
            }

            foreach (var cmetadata in _tree.BubblingControls)
            {
                await cmetadata.Control.HandleEvents(events);
            }
        }
    }
    
}