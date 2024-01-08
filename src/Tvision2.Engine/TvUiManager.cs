using Tvision2.Core.Engine.Components;
using Tvision2.Core.Engine.Render;

namespace Tvision2.Core.Engine;

public class TvUiNode
{
    public TvComponentTreeNode TreeRoot { get; }
    public IEnumerable<TvComponentTreeNode> FlattenedTree { get; private set; } = Enumerable.Empty<TvComponentTreeNode>();

    public TvUiNode(TvComponentTreeNode root)
    {
        TreeRoot = root;
    }

    internal void Flatten()
    {
        FlattenedTree = TreeRoot.SubTree().ToArray();
    }
}
public class TvUiManager
{
    private readonly TvComponentTree _tree;
    private readonly List<TvUiNode> _flattenedRoots;

    private readonly List<TvComponentTree> _roots;

    public IEnumerable<TvUiNode> Roots => _flattenedRoots;

    public TvUiManager()
    {
        _roots = new List<TvComponentTree>();
        _flattenedRoots = new List<TvUiNode>();
        _tree.On().RootAdded.Do(FlattenRootNode);
        _tree.On().NodeAdded.Do(FlattenNode);
    }

    public void AddRoot()
    {
        var newRoot = new TvComponentTree();
        newRoot.On().RootAdded.Do(FlattenRootNode);
        newRoot.On().NodeAdded.Do(FlattenNode);
        _roots.Add(newRoot);

    }

    private void FlattenRootNode(TvComponentTreeNode newRoot)
    {
        var uiNode = new TvUiNode(newRoot);
        uiNode.Flatten();
        _flattenedRoots.Add(uiNode);
    }

    private void FlattenNode(TvComponentTreeNode newNode)
    {
        if (!newNode.IsRoot)
        {
            var root = newNode.Root();
            var uiNode = _flattenedRoots.SingleOrDefault(r => r.TreeRoot == root);
            uiNode?.Flatten();
        }
    }

    internal void Draw(VirtualConsole console)
    {
        foreach (var root in _flattenedRoots)
        {
            foreach (var cmpNode in root.FlattenedTree)
            {
                var component = cmpNode.Component;
                
                if (component.Metadata.GetAndCleanIsDrawPending()) {
                    var layer = component.Layer.LayerIndex switch
                    {
                        0 => console.BottomLayer,
                        Int32.MaxValue => console.TopLayer,
                        -1 => console.StandardLayer,
                        int v => console.Layer(v)
                    };
                    component.Draw(layer);
                }
            }
        }
    }
    
    internal Task<bool> Update(bool forceDraws, TvConsoleEvents events)
    {
        var task = _tree.NewCycle()
            .ContinueWith(t =>
        {            
            var someDrawPending = false;
            var context = new UpdateContext(events);
            foreach (var root in _flattenedRoots)
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
}