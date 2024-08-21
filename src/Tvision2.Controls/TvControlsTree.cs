using System.Collections;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Components;
using static System.Net.Mime.MediaTypeNames;

namespace Tvision2.Controls;

public class TvControlsTree
{
    private readonly ITvComponentTree _componentTree;

    private readonly LinkedList<TvControlMetadata> _rootsByTabOrder = new();

    private LinkedListNode<TvControlMetadata>? _focusedRootNode = null;
    private ITvControl? _focusedControl = null;


    public TvControlsTree(Tvision2Engine engine)
    {
        _componentTree = engine.UI.ComponentTree;
        _componentTree.On().NodeAdded.Do(OnComponentAdded);
        _componentTree.On().RootAdded.Do(OnComponentAdded);
    }

    private void OnComponentAdded(TvComponentTreeNode node)
    {
        var metadata = node.Metadata;
        var controlMetadata = metadata.GetControlMetadata();
        if (controlMetadata is null) return;

        controlMetadata.AttachControl(this);
        if (node.IsRoot)
        {
            AddByTabOrder(controlMetadata);
        }
    }

    internal void AddByTabOrder(TvControlMetadata metadata)
    {
        if (_rootsByTabOrder.Count == 0)
        {
            _rootsByTabOrder.AddFirst(metadata);
        }
        else
        {
            var current = _rootsByTabOrder.First;
            while (current is not null)
            {
                if (current.Value.TabOrder > metadata.TabOrder)
                {
                    _rootsByTabOrder.AddBefore(current, metadata);
                    return;
                }
                current = current.Next;
            }
            _rootsByTabOrder.AddLast(metadata);
        }
    }

    public ITvControl? FocusedControl => _focusedControl;

    // Focus the specified control. This method makes checks that:
    //  1. Control is child of any root of the tree
    //  2. Control is contained in this controltree
    internal void SetFocusedControl(ITvControl? focused)
    {
        if (focused is null)
        {
            _focusedRootNode = null;
            return;
        }

        var rootValue = focused.Metadata.Node.Root();
        var rootMetadata = rootValue.Metadata.GetControlMetadata();

        if (rootMetadata is null)
        {
            throw new InvalidOperationException("[FATAL] Control is not root nor control of child???");
        }

        var rootNode = _rootsByTabOrder.Find(rootMetadata);
        if (rootNode is not null)
        {
            ChangeFocusedControlTo(rootNode, focused);
        }
        else
        {
            throw new InvalidOperationException("Control is not contained in this tree");
        }
    }

    // This is the same as setting FocusedControl but with no checks.
    // It's private so caller ensure checks are performed and ok.
    private void SetFocusedControl(TvControlMetadata focused)
    {
        var rootNode = focused.Node.Root();
        var focusedRootNode = _rootsByTabOrder.Find(rootNode.Metadata.GetControlMetadata()!)!;
        ChangeFocusedControlTo(focusedRootNode, focused.Control);
    }

    private void ChangeFocusedControlTo(LinkedListNode<TvControlMetadata> rootNode, ITvControl focused)
    {
        var oldFocused = _focusedControl;
        _focusedControl = focused;
        _focusedRootNode = rootNode;
        // Launch the lostfocus and gainedfocus events if possible
        if (oldFocused is ITvEventedControl evtControlPrevious)
        {
            evtControlPrevious.Raise().LostFocus();
        }

        if (_focusedControl is ITvEventedControl evtControlCurrent)
        {
            evtControlCurrent.Raise().GainedFocus();
        }
    }


    public IEnumerable<TvControlMetadata> TunnelingControls()
    {
        if (FocusedControl is null) yield break;
        var nodes = FocusedControl.AsComponent().Metadata.Node.PreOrder;
        foreach (var node in nodes)
        {
            var ctlMetadata = node.Metadata.GetControlMetadata();
            if (ctlMetadata is not null) yield return ctlMetadata;
        }
    }

    public IEnumerable<TvControlMetadata> BubblingControls()
    {
        if (FocusedControl is null) yield break;
        var nodes = FocusedControl.AsComponent().Metadata.Node.PostOrder;
        foreach (var node in nodes)
        {
            var ctlMetadata = node.Metadata.GetControlMetadata();
            if (ctlMetadata is not null) yield return ctlMetadata;
        }
    }

    internal void FocusNext()
    {
        var current = _focusedRootNode;
        var next = current;

        do
        {
            next = next?.Next ?? _rootsByTabOrder.First;
            if (next is null) return;

            var nextOptions = next.Value.ControlSetup;
            if (nextOptions.FocusPolicy == FocusPolicy.DirectFocusable)
            {
                break;
            }
        } while (next != current);

        if (next != current)
        {
            SetFocusedControl(next.Value);
        }
    }
}