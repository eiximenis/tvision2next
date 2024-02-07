namespace Tvision2.Controls;

public class TvControlMetadata
{

    public ITvControl Control { get; }
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