using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Layouts;

namespace Tvision2.Layouts;

public class TvContainer : ILayoutManager
{
    private readonly TvComponent _component;
    internal TvComponentMetadata ComponentMetadata => _component.Metadata;
    private readonly TvComponentMetadata _container;
    public TvContainer(TvComponentMetadata container, Viewport? viewport = null) : this(container, viewport, LayerSelector.Standard) { }

    public bool HasLayoutPending { get; private set; }

    public TvContainer(TvComponentMetadata container, Viewport? viewport, LayerSelector layerSelector)
    {
        _component = TvComponent.CreateStatelessComponent(layerSelector, viewport);
        _container = container;
        _container.On().ViewportUpdated.Do(OnContainerUpdated);
    }

    private void OnContainerUpdated(ViewportUpdateReason reason)
    {
        HasLayoutPending = true;
    }

    public ViewportUpdateReason UpdateLayout(TvComponentMetadata metadata)
    {
        var viewport = metadata.Component.Viewport;
        var (pos, bounds) = RecalculateBoundsAndPosition(viewport);
        viewport.MoveTo(pos);
        viewport.Resize(bounds);

        HasLayoutPending = false;
        return ViewportUpdateReason.None;
    }

    private (TvPoint Position, TvBounds Bounds) RecalculateBoundsAndPosition(Viewport viewportToUpdate)
    {
        return (viewportToUpdate.Position, viewportToUpdate.Bounds);
    }
}


/// <summary>
/// A container for TvComponents that is able to manage the layout of its children.
/// Type [TSate] is the type of the state that the container will manage.
/// </summary>
public class TvContainer<TState> : TvContainer
{


}
