namespace Tvision2.Engine.Components;

public class TvComponentTreeNode
{
    private readonly LinkedList<TvComponentTreeNode> _childs;
    public TvComponentMetadata Metadata { get; }
    public TvComponentTreeNode? Parent { get; private set; }
    public bool IsRoot => Parent is null;
    private TvComponentTreeNode[] _flattened;
    private Dictionary<string, object> _tags;
    
    internal TvComponentTreeNode(TvComponentMetadata metadata)
    {
        _childs = new LinkedList<TvComponentTreeNode>();
        _flattened = new[] { this };
        Metadata = metadata;
        _tags = new Dictionary<string, object>();
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

    internal void AddChild(TvComponentTreeNode child)
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

    internal void SetTag<T>(string key, T data) where T : class
    {
        _tags[key] = data;
    }

    internal bool HasTag(string key) => _tags.ContainsKey(key);

    internal T? GetTag<T>(string key) where T : class => _tags.TryGetValue(key, out var result) ? (T)result : null;
}