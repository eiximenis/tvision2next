namespace Tvision2.Core.Engine.Components;

public class TvComponentMetadata
{
    private readonly TvComponent _owner;
    private readonly TvComponentTreeNode _node;
    internal TvComponentMetadata(TvComponent owner)
    {
        _owner = owner;
        _node = new TvComponentTreeNode(this);
    }
    public void AddChild(TvComponent child)
    {
        _node.AddChild(child.Metadata._node);
    }
}