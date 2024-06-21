using System.Runtime.CompilerServices;
using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public class TvControlMetadata
{
    private int _tabOrder;
    public bool IsAttached => _tree is not null;

    public TvComponentTreeNode Node => Control.AsComponent().Metadata.Node;
    public ITvControl Control { get; }

    public int TabOrder
    {
        get => _tabOrder;
        set
        {
            if (IsAttached && Control.AsComponent().Metadata.Node.IsRoot &&  _tabOrder != value)
            {
                // TODO: Resort tab orders!!!!!!
            }
        }
    }

    private TvControlsTree? _tree;

    public TvControlMetadata(ITvControl owner)
    {
        Control = owner;
        _tree = null;
    }

    internal void AttachControl(TvControlsTree tvControlsTree)
    {
        if (_tree is not null && _tree != tvControlsTree)
        {
            throw new InvalidOperationException("Control is already attached to another tree!");
        }
        _tree = tvControlsTree;
    }

    internal bool Focus()
    {
        if (_tree is null) return false;
        _tree.SetFocusedControl(Control);
        return true;
    }
}