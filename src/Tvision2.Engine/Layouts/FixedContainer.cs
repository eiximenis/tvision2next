using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;

namespace Tvision2.Engine.Layouts;

/// <summary>
/// This class is a container that has a fixed viewport. It is useful to create layouts that do not need to be recalculated.
/// This container never changes its viewport once created thus never notifies viewport updates.
/// </summary>
class FixedContainer : ITvContainer, ITvContainerActions
{
    private readonly ActionsChain<ViewportUpdateReason> _viewportUpdated = new();
    private readonly Viewport _viewport;
    public FixedContainer(Viewport fixedViewport)
    {
        _viewport = new Viewport(fixedViewport.Position, fixedViewport.Bounds);
    }

    public ITvContainerActions On() => this;

    public IViewportSnapshot Viewport => _viewport;
    public IActionsChain<ViewportUpdateReason> ViewportUpdated => _viewportUpdated;
}

public static class ViewportLayout_Extensions
{
    public static ITvContainer AsFixedContainer(this Viewport viewport) => new FixedContainer(viewport);
}