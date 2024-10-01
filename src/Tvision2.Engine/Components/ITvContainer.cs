using Tvision2.Engine.Events;

namespace Tvision2.Engine.Components;

public interface ITvContainerActions
{
    IActionsChain<ViewportUpdateReason> ViewportUpdated { get; }
}

/// <summary>
/// A Container is something that has provides a viewport and notifies when the viewport is updated.
/// It's the base for layouting system (layouts work attaching components to containers).
/// Note that every component is a container through its metadata, but containers that are not components are also possible.
/// </summary>
public interface ITvContainer
{
    ITvContainerActions On();
    IViewportSnapshot Viewport { get; }
}