using Tvision2.Core.Engine.Events;
using Tvision2.Engine.Components;

namespace Tvision2.Core.Engine.Components;

public class TvComponentTreeNode
{
    private readonly LinkedList<TvComponentTreeNode> _childs;
    public TvComponentMetadata ComponentData { get; }
    public TvComponentTreeNode? Parent { get; private set; }
    public bool IsRoot => Parent is null;


    public TvComponentTreeNode(TvComponentMetadata metadata)
    {
        _childs = new LinkedList<TvComponentTreeNode>();
        ComponentData = metadata;
    }
    
    public TvComponentTreeNode Root()
    {
        var node = this;
        while (node.Parent is not null)
        {
            node = node.Parent;
        }
        return node;
    }

    public void AddChild(TvComponentTreeNode child)
    {
        _childs.AddLast(child);
        child.Parent = this;
    }
    
    public void RemoveChild(TvComponentTreeNode child)
    {
        if (_childs.Remove(child))
        {
            child.Parent = null;
        }
    }
    
    public IEnumerable<TvComponentTreeNode> Descendants() =>
        _childs.SelectMany(c => c.SubTree());
    
    public IEnumerable<TvComponentTreeNode> SubTree() =>
        _childs.SelectMany(c => c.SubTree()).Union(new[] { this });
}


public interface ITvComponentTreeActions
{
    IActionsChain<TvComponentTreeNode> NodeAdded { get; }
    IActionsChain<TvComponentTreeNode> RootAdded { get; }
        
    IActionsChain<Unit> TreeUpdated { get; } 
}

class TvComponentTree  :  ITvComponentTreeActions
{
    private readonly List<TvComponentTreeNode> _roots;
    private readonly ActionsChain<TvComponentTreeNode> _onNodeAdded;
    private readonly ActionsChain<TvComponentTreeNode> _onRootAdded;
    private readonly ActionsChain<Unit> _onTreeUpdated;
    private bool _dirty;
    private readonly Dictionary<Type, object> _sharedTags;

    public IEnumerable<TvComponentTreeNode> Roots { get => _roots; }

    public ITvComponentTreeActions On() => this;
    IActionsChain<TvComponentTreeNode> ITvComponentTreeActions.NodeAdded => _onNodeAdded;
    IActionsChain<TvComponentTreeNode> ITvComponentTreeActions.RootAdded => _onRootAdded;
    IActionsChain<Unit> ITvComponentTreeActions.TreeUpdated => _onTreeUpdated;

    public TvComponentTree()
    {
        _dirty = false;
        _sharedTags = new Dictionary<Type, object>();
        _roots = new List<TvComponentTreeNode>();
        _onNodeAdded = new ActionsChain<TvComponentTreeNode>();
        _onRootAdded = new ActionsChain<TvComponentTreeNode>();
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
        var rootMetadata = component.Metadata;
        _roots.Add(rootMetadata.Node);
        await rootMetadata.AttachToTree(this);
        var subtree = rootMetadata.Node.Descendants();
        foreach (var node in subtree)
        {
            await node.Component.Metadata.AttachToTree(rootMetadata.OwnerTree);
        }

        await _onRootAdded.Invoke(rootMetadata.Node);
        await _onNodeAdded.Invoke(rootMetadata.Node);
        _dirty = true;

        return rootMetadata.Node;
    }

    public Task<TvComponentTreeNode> AddChild(TvComponent child, TvComponent parent) => AddChild(child, parent, LayerSelector.Standard);
    public async Task<TvComponentTreeNode> AddChild(TvComponent child, TvComponent parent, LayerSelector layer)
    {
        child.UseLayer(layer);
        var childMetadata = child.Metadata;
        await parent.Metadata.AddChild(child);
        await _onNodeAdded.Invoke(childMetadata.Node);
        _dirty = true;
        
        return childMetadata.Node;
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