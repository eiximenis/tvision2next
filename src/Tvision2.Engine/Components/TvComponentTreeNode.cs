namespace Tvision2.Engine.Components;

public class TvComponentTreeNode
{
    private readonly LinkedList<TvComponentTreeNode> _childs;
    public TvComponentMetadata Metadata { get; }
    public TvComponentTreeNode? Parent { get; private set; }
    public bool IsRoot => Parent is null;
    private TvComponentTreeNode[] _flattened;
    private Dictionary<string, object> _tags;

    private readonly List<TvComponentTreeNode> _postOrder;
    private readonly List<TvComponentTreeNode> _preOrder;
    
    internal TvComponentTreeNode(TvComponentMetadata metadata)
    {
        _childs = new LinkedList<TvComponentTreeNode>();
        _flattened = new[] { this };
        Metadata = metadata;
        _tags = new Dictionary<string, object>();
        _postOrder = new List<TvComponentTreeNode>() { this };
        _preOrder = new List<TvComponentTreeNode>() { this };
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


    private void StartPostOrder()
    {
        _postOrder.Clear();
        TraversePostorder(_postOrder);
    }
    private void TraversePostorder(ICollection<TvComponentTreeNode> nodes)
    {
        foreach (var child in _childs)
        {
            child.TraversePostorder(nodes);
        }
        nodes.Add(this);
    }

    private void StartPreOrder()
    {
        _preOrder.Clear();
        TraversePreorder(_preOrder);
    }

    private void TraversePreorder(ICollection<TvComponentTreeNode> nodes)
    {
        nodes.Add(this);
        foreach (var child in _childs)
        {
            child.TraversePreorder(nodes);
        }
    }

    private void Flatten()
    {
        _flattened = SubTree().ToArray();
        StartPreOrder();
        StartPostOrder();
    }

    public IEnumerable<TvComponentTreeNode> Descendants() =>
        _childs.SelectMany(c => c.SubTree());

    public IEnumerable<TvComponentTreeNode> SubTree() =>
        _childs.SelectMany(c => c.SubTree()).Union(new[] { this });

    public IEnumerable<TvComponentTreeNode> PreOrder => _preOrder;
    public IEnumerable<TvComponentTreeNode> PostOrder => _postOrder;

    public IEnumerable<TvComponentTreeNode> FlattenedTree => _flattened;

    internal void SetTag<T>(string key, T data) where T : class
    {
        _tags[key] = data;
    }

    internal bool HasTag(string key) => _tags.ContainsKey(key);

    internal T? GetTag<T>(string key) where T : class => _tags.TryGetValue(key, out var result) ? (T)result : null;
}