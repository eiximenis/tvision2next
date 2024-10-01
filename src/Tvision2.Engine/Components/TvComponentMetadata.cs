using Tvision2.Core;
using Tvision2.Engine.Events;

namespace Tvision2.Engine.Components;




public interface ITvComponentMetadataActions
{
    IActionsChain<ViewportUpdateReason> ViewportUpdated { get; }
    IActionsChain<Unit> ComponentRemoved { get; }
}

/// <summary>
/// Holds extra metadata for a TvComponent. Also implements ITvContainer to allow components to be containers.
/// </summary>
public class TvComponentMetadata : ITvComponentMetadataActions
{
    private readonly TvComponent _owner;
    private readonly TvComponentTreeNode _node;
    private TvComponentTree? _ownerTree;
    public bool IsAttached { get; private set; }
    public TvComponentTreeNode Node { get => _node; }
    public TvComponent Component => _owner;
    private readonly ActionsChain<ViewportUpdateReason> _viewportUpdated;
    private readonly ActionsChain<Unit> _componentRemoved;

    public ITvComponentTree? OwnerTree => _ownerTree;

    public ITvComponentMetadataActions On() => this;
    IActionsChain<ViewportUpdateReason> ITvComponentMetadataActions.ViewportUpdated => _viewportUpdated;
    IActionsChain<Unit> ITvComponentMetadataActions.ComponentRemoved => _componentRemoved;

    public void TagWith<T>(string tag, T data) where T : class => _node.SetTag(tag, data);
    public bool HasTag(string tag) => _node.HasTag(tag);
    public T? GetTag<T>(string tag) where T : class => _node.GetTag<T>(tag);

    public T GetOrCreateTag<T>(string tag, Func<T> dataCreator) where T : class
    {
        if (HasTag(tag))
        {
            return GetTag<T>(tag)!;
        }

        var data = dataCreator();
        TagWith(tag, data);
        return data;
    }


    internal TvComponentMetadata(TvComponent owner)
    {
        _owner = owner;
        _node = new TvComponentTreeNode(this);
        IsAttached = false;
        _ownerTree = null;
        _viewportUpdated = new ActionsChain<ViewportUpdateReason>();
        _componentRemoved = new ActionsChain<Unit>();
    }
    internal void AddChild(TvComponent child)
    {
        _node.AddChild(child.Metadata._node);
        if (IsAttached)
        {
            child.Metadata.AttachToTree(_ownerTree!); 
        }
    }

    internal void AttachToTree(TvComponentTree owner)
    {
        if (IsAttached && owner != _ownerTree)
        {
            throw new InvalidOperationException("Component is already attached to another tree");
        }
        _ownerTree = owner;
        IsAttached = true;
    }
    
    internal async Task DettachFromTree(TvComponentTree owner)
    {
        if (!IsAttached) return;
        
        if (owner != _ownerTree) throw new InvalidOperationException("Component is attached to a different tree");
        IsAttached = false;
        _ownerTree = null;
        await _componentRemoved.Invoke(Unit.Value);
    }

    internal async Task RaiseViewportUpdated(ViewportUpdateReason reason)
    {
        if (IsAttached)
        {
            await _viewportUpdated.Invoke(reason);
        }
    }
}