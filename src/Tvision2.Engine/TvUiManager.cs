using Tvision2.Core.Console;
using Tvision2.Engine.Components;
using Tvision2.Engine.Components.Events;
using Tvision2.Engine.Render;

namespace Tvision2.Engine;

public class TvUiManager
{
    private readonly TvComponentTree _tree;
    public ITvComponentTree ComponentTree => _tree;
    private readonly VirtualConsole _console;
    private readonly IConsoleDriver _consoleDriver;
    private readonly List<TvComponent> _componentsToDraw;

    public TvUiManager(VirtualConsole console, IConsoleDriver consoleDriver)
    {
        _tree = new TvComponentTree();
        _console = console;
        _consoleDriver = consoleDriver;
        _componentsToDraw = new List<TvComponent>();
    }
    

    internal void Draw()
    {
        foreach (var cmpNode in _tree.ByLayerBottomFirst)
        {
            var metadata = cmpNode.ComponentData;
            var component = metadata.Component;
            component.Draw(_console);
        }

        _console.Flush(_consoleDriver);
    }

    internal async Task<bool> Update(TvConsoleEvents events)
    {
        await _tree.NewCycle();
        var someDrawPending = false;
        foreach (var cmpNode in _tree.ByLayerBottomFirst)
        {
            var metadata = cmpNode.ComponentData;
            var component = metadata.Component;
            var result = component.Update(events);
            someDrawPending = result != DirtyStatus.Clean;
            // TODO: We could calculate pending draws to call only Draw() on components that are needed.
        }
        return someDrawPending;
    }
}