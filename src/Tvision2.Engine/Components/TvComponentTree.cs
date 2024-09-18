using System.ComponentModel;
using System.Reflection.Emit;
using Tvision2.Core;
using Tvision2.Engine.Events;

namespace Tvision2.Engine.Components;

public interface ITvComponentTreeActions
{
    IActionsChain<TvComponentTreeNode> RootAdded { get; }
    IActionsChain<TvComponentTreeNode> NodeAdded { get; }
    IActionsChain<Unit> TreeUpdated { get; } 
}

public interface ITvComponentTree
{
    ITvComponentTreeActions On();
    void Add(TvComponent component);
    void Add(TvComponent component, LayerSelector layer);
    void AddChild(TvComponent child, TvComponent parent);
    void AddChild(TvComponent child, TvComponent parent, LayerSelector layer);
    void Remove(TvComponent component);
    IEnumerable<TvComponentTreeNode> Roots { get; }
    IEnumerable<TvComponentTreeNode> ByLayerTopFirst { get; }
    void AddSharedTag<TTag>(TTag tag) where TTag : class;
    TTag? GetSharedTag<TTag>();
    bool HasTag<TTag>();

}

class TvComponentTree  :  ITvComponentTreeActions, ITvComponentTree
{

    record AddParams(TvComponent component, LayerSelector layer);
    record ChildAddParams(TvComponent child, TvComponent parent, LayerSelector layer);
    private readonly List<TvComponentTreeNode> _roots;
    private readonly ActionsChain<TvComponentTreeNode> _onRootAdded;
    private readonly ActionsChain<TvComponentTreeNode> _onNodeAdded;
    private readonly ActionsChain<Unit> _onTreeUpdated;
    private bool _dirty;
    private readonly Dictionary<Type, object> _sharedTags;
    private readonly List<TvComponentTreeNode> _sortedNodesByLayerTopFirst;

    private readonly List<AddParams> _pendingAdds;
    private readonly List<ChildAddParams> _pendingChildAdds;
    private readonly List<TvComponent> _pendingRemoves;

    public IEnumerable<TvComponentTreeNode> Roots => _roots;

    public ITvComponentTreeActions On() => this;
    IActionsChain<TvComponentTreeNode> ITvComponentTreeActions.RootAdded => _onRootAdded;
    IActionsChain<TvComponentTreeNode> ITvComponentTreeActions.NodeAdded => _onNodeAdded;
    IActionsChain<Unit> ITvComponentTreeActions.TreeUpdated => _onTreeUpdated;

    public IEnumerable<TvComponentTreeNode> ByLayerTopFirst => _sortedNodesByLayerTopFirst;

    
    public TvComponentTree()
    {
        _dirty = false;
        _sharedTags = new Dictionary<Type, object>();
        _sortedNodesByLayerTopFirst = new List<TvComponentTreeNode>();
        _roots = new List<TvComponentTreeNode>();
        _pendingAdds = new List<AddParams>();
        _pendingChildAdds = new List<ChildAddParams>();
        _pendingRemoves = new List<TvComponent>();
        _onRootAdded = new ActionsChain<TvComponentTreeNode>();
        _onNodeAdded = new ActionsChain<TvComponentTreeNode>();
        _onTreeUpdated = new ActionsChain<Unit>();
    }
    

    internal async Task NewCycle()
    {
        await DoPengingAdds();
        await DoPendingChildAdds();
        await DoPendingRemoves();
        if (_dirty)
        {
            _dirty = false;
            await _onTreeUpdated.Invoke(Unit.Value);
        }
    }

    private async Task DoPendingRemoves()
    {
        if (_pendingRemoves.Count == 0) return;

        foreach (var cmpToRemove in _pendingRemoves)
        {
            var node = cmpToRemove.Metadata.Node;
            if (node.IsRoot)
            {
                _roots.Remove(node);
            }

            if (_sortedNodesByLayerTopFirst.Contains(node))
            {
                _sortedNodesByLayerTopFirst.Remove(node);
                await node.Metadata.DettachFromTree(this);
            }
        }
    }

    private async Task DoPengingAdds()
    {
        if (_pendingAdds.Count == 0) return;
        foreach (var (component, layer) in _pendingAdds)
        {
            component.MoveToLayer(layer);
            var rootMetadata = component.Metadata;
            var rootNode = rootMetadata.Node;
            _roots.Add(rootNode);
            await DoAddSubtree(rootNode, layer);
            await _onRootAdded.Invoke(rootMetadata.Node);
            _dirty = true;
        }

        _pendingAdds.Clear();
    }

    private async Task DoPendingChildAdds()
    {
        if (_pendingChildAdds.Count == 0) return;
        foreach (var (child, parent, layer) in _pendingChildAdds)
        {
            var childMetadata = child.Metadata;
            var parentMetadata = parent.Metadata;
            parentMetadata.AddChild(child);
            var childNode = childMetadata.Node;
            if (parentMetadata.IsAttached)
            {
                await DoAddSubtree(childNode, layer);
            }
        }
    }

    public void Add(TvComponent component) => Add(component, LayerSelector.Standard);
    public void Add(TvComponent component, LayerSelector layer)
    {
        _pendingAdds.Add(new AddParams(component, layer));
    }

    private async Task DoAddSubtree(TvComponentTreeNode stRoot, LayerSelector layer)
    {
        foreach (var node in stRoot.SubTree())
        {
            var metadata = node.Metadata;
            if (metadata.IsAttached) continue;            // If node is already attached we skip it

            var component = metadata.Component;
            if (component.Layer.IsNone)
            {
                component.MoveToLayer(layer);
            }

            _sortedNodesByLayerTopFirst.Add(node);
            metadata.AttachToTree(this);
            if (!node.IsRoot)
            {
                await _onNodeAdded.Invoke(node);
            }
        }

        _sortedNodesByLayerTopFirst.Sort(NodeWithTopComponentFirst);
    }

    public void Remove(TvComponent component)
    {
        if (component.Metadata.OwnerTree == this)
        {
            _pendingRemoves.Add(component);
        }
    }


    private static int NodeWithTopComponentFirst(TvComponentTreeNode n1, TvComponentTreeNode n2)
    {
        return LayerSelector.CompareTopFirst(n1.Metadata.Component.Layer, n2.Metadata.Component.Layer);
    }

    public void AddChild(TvComponent child, TvComponent parent) => AddChild(child, parent, LayerSelector.Standard);
    
    /// <summary>
    /// Adds a component as a child of another component. This **does not** add the parent component to the tree.
    /// </summary>
    public void AddChild(TvComponent child, TvComponent parent, LayerSelector layer)
    {
        if (child == parent) throw new ArgumentException("Cannot add a component as a child of itself");
        var childMetadata = child.Metadata;
        if (childMetadata.IsAttached) throw new InvalidOperationException("Cannot add a component that is already attached to a tree");

        _pendingChildAdds.Add(new ChildAddParams(child, parent, layer));

    }


    public IEnumerable<TvComponentTreeNode> Nodes() => _roots.SelectMany(r => r.SubTree());

    public void AddSharedTag<TTag>(TTag tag) where TTag : class
    {
        ArgumentNullException.ThrowIfNull(tag);
        _sharedTags.Add(typeof(TTag), tag);   
    }

    public TTag? GetSharedTag<TTag>() => _sharedTags.TryGetValue(typeof(TTag), out var tag) ? (TTag)tag : default;

    public bool HasTag<TTag>() => _sharedTags.ContainsKey(typeof(TTag));
    
}