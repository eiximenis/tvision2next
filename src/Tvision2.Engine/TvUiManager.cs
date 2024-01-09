using Tvision2.Core.Console;
using Tvision2.Core.Engine.Components;
using Tvision2.Core.Engine.Render;

namespace Tvision2.Core.Engine;

public class TvUiManager
{
    private readonly TvComponentTree _tree;
    public ITvComponentTree ComponentTree => _tree;
    private readonly VirtualConsole _console;

    public TvUiManager(ConsoleOptions options)
    {
        _tree = new TvComponentTree();
        _console = new VirtualConsole(TvBounds.FromRowsAndCols(24,80), TvColor.Black);
    }
    

    internal void Draw(VirtualConsole console)
    {
        foreach (var root in _tree.Roots)
        {
            foreach (var cmpNode in root.FlattenedTree)
            {
                var metadata = cmpNode.ComponentData;
                var component = metadata.Component;
                if (metadata.GetAndCleanIsDrawPending())
                {
                    component.Draw(_console);
                }
            }   
        }
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