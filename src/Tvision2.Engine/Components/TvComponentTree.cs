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
    Task<TvComponentTreeNode> Add(TvComponent component);
    Task<TvComponentTreeNode> Add(TvComponent component, LayerSelector layer);
    Task<TvComponentTreeNode> AddChild(TvComponent child, TvComponent parent);
}

class TvComponentTree  :  ITvComponentTreeActions, ITvComponentTree
{
    private readonly List<TvComponentTreeNode> _roots;
    private readonly ActionsChain<TvComponentTreeNode> _onRootAdded;
    private readonly ActionsChain<TvComponentTreeNode> _onNodeAdded;
    private readonly ActionsChain<Unit> _onTreeUpdated;
    private bool _dirty;
    private readonly Dictionary<Type, object> _sharedTags;
    private readonly List<TvComponentTreeNode> _sortedNodes;

    public IEnumerable<TvComponentTreeNode> Roots { get => _roots; }

    public ITvComponentTreeActions On() => this;
    IActionsChain<TvComponentTreeNode> ITvComponentTreeActions.RootAdded => _onRootAdded;
    IActionsChain<TvComponentTreeNode> ITvComponentTreeActions.NodeAdded => _onNodeAdded;
    IActionsChain<Unit> ITvComponentTreeActions.TreeUpdated => _onTreeUpdated;

    public IEnumerable<TvComponentTreeNode> ByLayerBottomFirst => _sortedNodes;

    public TvComponentTree()
    {
        _dirty = false;
        _sharedTags = new Dictionary<Type, object>();
        _sortedNodes = new List<TvComponentTreeNode>();
        _roots = new List<TvComponentTreeNode>();
        _onRootAdded = new ActionsChain<TvComponentTreeNode>();
        _onNodeAdded = new ActionsChain<TvComponentTreeNode>();
        _onTreeUpdated = new ActionsChain<Unit>();
    }
    

    internal async Task NewCycle()
    {
        if (_dirty)
        {
            _dirty = false;
            await _onTreeUpdated.Invoke(Unit.Value);
        }
    }
    
    public Task<TvComponentTreeNode> Add(TvComponent component) => Add(component, LayerSelector.Standard);
    public async Task<TvComponentTreeNode> Add(TvComponent component, LayerSelector layer)
    {
        component.UseLayer(layer);
        var rootMetadata = component.Metadata;
        var rootNode = rootMetadata.Node;
        _roots.Add(rootNode);
        await DoAddSubtree(rootNode, layer);
        await _onRootAdded.Invoke(rootMetadata.Node);
        _dirty = true;
        return rootMetadata.Node;
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
                component.UseLayer(layer);
            }

            _sortedNodes.Add(node);
            metadata.AttachToTree(this);
            if (!node.IsRoot)
            {
                await _onNodeAdded.Invoke(node);
            }
        }

        _sortedNodes.Sort(NodeWithBottomComponentFirst);
    }


    private static int NodeWithBottomComponentFirst(TvComponentTreeNode n1, TvComponentTreeNode n2)
    {
        return LayerSelector.CompareBottomFirst(n1.Metadata.Component.Layer, n2.Metadata.Component.Layer);
    }

    public async Task<TvComponentTreeNode> AddChild(TvComponent child, TvComponent parent) =>
        await AddChild(child, parent, LayerSelector.Standard);
    
    /// <summary>
    /// Adds a component as a child of another component. This **does not** add the parent component to the tree.
    /// </summary>
    public async Task<TvComponentTreeNode> AddChild(TvComponent child, TvComponent parent, LayerSelector layer)
    {
        if (child == parent) throw new ArgumentException("Cannot add a component as a child of itself");
        var childMetadata = child.Metadata;
        if (childMetadata.IsAttached) throw new InvalidOperationException("Cannot add a component that is already attached to a tree");
        var parentMetadata = parent.Metadata;
        parentMetadata.AddChild(child);
        var childNode = childMetadata.Node;
        if (parentMetadata.IsAttached)
        {
            await DoAddSubtree(childNode, layer);
        }
        return childNode;
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