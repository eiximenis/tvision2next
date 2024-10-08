using System.Reflection;
using System.Runtime.CompilerServices;
using Tvision2.Controls.Extensions;
using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public class TvControlMetadata
{
    private int _tabOrder;
    private TvControlsTree? _tree;
    public bool IsAttached => _tree is not null;
    public ITvControl Control { get; }
    public TvComponentTreeNode Node => Control.AsComponent().Metadata.Node;
    public bool IsFocused => IsAttached && _tree!.FocusedControl == Control;
    internal TvControlSetup ControlSetup { get; }

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

    internal TvControlMetadata(ITvControl owner, TvControlSetup controlOptions)
    {
        Control = owner;
        _tree = null;
        ControlSetup = controlOptions;
    }


    internal void AttachControl(TvControlsTree tvControlsTree)
    {
        if (_tree is not null && _tree != tvControlsTree)
        {
            throw new InvalidOperationException("Control is already attached to another tree!");
        }
        _tree = tvControlsTree;
    }

    internal bool TryFocus()
    {
        var focusPolicy = Control.Metadata.ControlSetup.FocusPolicy;

        if (focusPolicy == FocusPolicy.NotFocusable || _tree is null) return false;
        _tree.SetFocusedControl(Control);
        return true;
    }
}

public enum FocusPolicy
{
    DirectFocusable,
    NotFocusable
}