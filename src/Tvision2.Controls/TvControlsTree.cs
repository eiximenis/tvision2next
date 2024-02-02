using System.Collections;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public class TvControlsTree
{
    private readonly ITvComponentTree _componentTree;

    private readonly List<TvControlMetadata> _tunnelingControls;
    private readonly List<TvControlMetadata> _bubblingControls;
    
    public TvControlsTree(Tvision2Engine engine)
    {
        _tunnelingControls = new List<TvControlMetadata>();
        _bubblingControls = new List<TvControlMetadata>();
        _componentTree = engine.UI.ComponentTree;
        _componentTree.On().TreeUpdated.Do(OnComponentTreeUpdated);
    }

    public ITvControl? FocusedControl { get; private set; }
    public IEnumerable<TvControlMetadata> TunnelingControls => _tunnelingControls;
    public IEnumerable<TvControlMetadata> BubblingControls => _bubblingControls;

    private void OnComponentTreeUpdated(Unit _)
    {
        _tunnelingControls.Clear();
        foreach (var root in _componentTree.Roots)
        {
            foreach (var node in root.PreOrder)
            {
                var controlMetadata = node.Metadata.GetTag<TvControlMetadata>(TvControl.CONTROL_TAG);
                if (controlMetadata is not null)
                {
                    _tunnelingControls.Add(controlMetadata!);
                }
            }
        }

        _bubblingControls.Clear();
        foreach (var root in _componentTree.Roots)
        {
            foreach (var node in root.PostOrder)
            {
                var controlMetadata = node.Metadata.GetTag<TvControlMetadata>(TvControl.CONTROL_TAG);
                if (controlMetadata is not null)
                {
                    _bubblingControls.Add(controlMetadata!);
                }
            }
        }
    }
}