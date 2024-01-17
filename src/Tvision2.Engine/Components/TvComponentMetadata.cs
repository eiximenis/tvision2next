namespace Tvision2.Engine.Components;

public class TvComponentMetadata
{
    private readonly TvComponent _owner;
    private readonly TvComponentTreeNode _node;
    private TvComponentTree? _ownerTree;
    internal bool IsAttached { get; private set; }
    public TvComponentTreeNode Node { get => _node; }
    public TvComponent Component => _owner;
    internal TvComponentMetadata(TvComponent owner)
    {
        _owner = owner;
        _node = new TvComponentTreeNode(this);
        IsAttached = false;
        _ownerTree = null;
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
    
    internal void DettachFromTree(TvComponentTree owner)
    {
        if (!IsAttached) return;
        
        if (owner != _ownerTree) throw new InvalidOperationException("Component is attached to a different tree");
        IsAttached = false;
        _ownerTree = null;
    }
    
}