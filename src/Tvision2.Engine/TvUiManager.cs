using Tvision2.Console.Events;
using Tvision2.Core.Console;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Engine;

public class TvUiManager
{
    private readonly TvComponentTree _tree;
    public ITvComponentTree ComponentTree => _tree;
    private readonly VirtualConsole _console;
    private readonly IConsoleDriver _consoleDriver;
    private readonly List<IHook> _hooks;

    public TvUiManager(VirtualConsole console, IConsoleDriver consoleDriver)
    {
        _tree = new TvComponentTree();
        _console = console;
        _consoleDriver = consoleDriver;
        _hooks = new List<IHook>();
    }


    internal async Task BeforeUpdate(UpdateContext ctx)
    {
        var events = ctx.ConsoleEvents;
        foreach (var hook in _hooks)
        {
            await hook.BeforeUpdate(events);
        }
    }

    internal async Task<bool> Update(UpdateContext ctx)
    {
        await _tree.NewCycle();
        var someDrawPending = false;
        foreach (var cmpNode in _tree.ByLayerTopFirst)
        {
            var metadata = cmpNode.Metadata;
            var component = metadata.Component;
            var result = component.Update(ctx);
            someDrawPending = result != DirtyStatus.Clean;
        }
        return someDrawPending;
    }

    internal async Task CalculateLayout()
    {
        foreach (var cmpNode in _tree.ByLayerTopFirst)
        {
            var cmp = cmpNode.Metadata.Component;
            if (cmp.Viewport.HasLayoutPending || cmp.Layout.HasLayoutPending)
            {
                var updated = cmp.Layout.UpdateLayout(cmp.Viewport);
                cmp.Viewport.LayoutUpdated();
                if (updated != ViewportUpdateReason.None)
                {
                    await cmp.Metadata.RaiseViewportUpdated(updated);
                }
            }
        }
    }

    internal void Draw()
    {
        var currentLayer = LayerSelector.Top;

        foreach (var cmpNode in _tree.ByLayerTopFirst)
        {
            var metadata = cmpNode.Metadata;
            var component = metadata.Component;
            if (component.Layer < currentLayer)
            {
                currentLayer = component.Layer;
                _console.StartNewLayer();
            }
            component.Draw(_console);
        }

        _console.Flush(_consoleDriver);
    }

    public void AddHook(IHook hook)
    {
        _hooks.Add(hook);
    }

    internal async Task Teardown()
    {
        foreach (var hook in _hooks)
        {
            await hook.Teardown();
        }
    }

    internal async Task Init()
    {
        foreach (var hook in _hooks)
        {
            await hook.Init();
        }
    }
}