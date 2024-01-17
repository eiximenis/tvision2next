namespace Tvision2.Engine.Components;

public class TvComponentTreeNode
{
    private readonly LinkedList<TvComponentTreeNode> _childs;
    public TvComponentMetadata ComponentData { get; }
    public TvComponentTreeNode? Parent { get; private set; }
    public bool IsRoot => Parent is null;
    private TvComponentTreeNode[] _flattened;
    
    public TvComponentTreeNode(TvComponentMetadata metadata)
    {
        _childs = new LinkedList<TvComponentTreeNode>();
        _flattened = new[] { this };
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
        Root().Flatten();
    }

    public void RemoveChild(TvComponentTreeNode child)
    {
        if (_childs.Remove(child))
        {
            child.Parent = null;
            Root().Flatten();
        }
    }

    private void Flatten()
    {
        _flattened = SubTree().ToArray();
    }

    public IEnumerable<TvComponentTreeNode> Descendants() =>
        _childs.SelectMany(c => c.SubTree());

    public IEnumerable<TvComponentTreeNode> SubTree() =>
        _childs.SelectMany(c => c.SubTree()).Union(new[] { this });

    public IEnumerable<TvComponentTreeNode> FlattenedTree => _flattened;
    
}