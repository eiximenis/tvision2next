using System.Collections;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public class TvControlsTree
{
    private readonly ITvComponentTree _componentTree;
    
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
        if (controlMetadata is not null)
        {
            controlMetadata.AttachControl(this);
        }
    }

    public ITvControl? FocusedControl { get; private set; }
    
    internal void SetFocusedControl(ITvControl focused) => FocusedControl = focused;

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

}