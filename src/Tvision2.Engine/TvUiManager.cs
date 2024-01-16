using Tvision2.Core.Console;
using Tvision2.Core.Engine.Components;
using Tvision2.Core.Engine.Render;
using Tvision2.Engine.Render;

namespace Tvision2.Core.Engine;

public class TvUiManager
{
    private readonly TvComponentTree _tree;
    public ITvComponentTree ComponentTree => _tree;
    private readonly VirtualConsole _console;
    private readonly IConsoleDriver _consoleDriver;

    public TvUiManager(VirtualConsole console, IConsoleDriver consoleDriver)
    {
        _tree = new TvComponentTree();
        _console = console;
        _consoleDriver = consoleDriver;
    }
    

    internal void Draw()
    {
        foreach (var root in _tree.Roots)
        {
            foreach (var cmpNode in root.FlattenedTree)
            {
                var metadata = cmpNode.ComponentData;
                var component = metadata.Component;
                component.Draw(_console);
            }   
        }

        _console.Flush(_consoleDriver);
    }
    
    /*
    internal Task<bool> Update(bool forceDraws, TvConsoleEvents events)
    {
        var task = _tree.NewCycle()
            .ContinueWith(t =>
        {            
            var someDrawPending = false;
            var context = new UpdateContext(events);
            foreach (var root in _roots)
            {
                foreach (var cmpNode in root.FlattenedTree)
                {
                    var result = cmpNode.Component.Update(context);
                    if (result == UpdateResult.Dirty || forceDraws)
                    {
                        cmpNode.Component.Metadata.SetDrawPending();
                    }
                    someDrawPending = someDrawPending || cmpNode.Component.Metadata.HasDrawPending();
                }
            }
            return someDrawPending;
        });
        return task;
    }
    */
}