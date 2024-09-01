using Tvision2.Console.Events;
using Tvision2.Controls.Button;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;

namespace Tvision2.Controls;

/// <summary>
/// Basic interface all controls must implement.
/// </summary>
public interface ITvControl
{
    /// <summary>
    /// Returns the control as a component, allowing interop with the core component system.
    /// </summary>
    /// <returns></returns>
    TvComponent AsComponent();
    /// <summary>
    /// Set focus on the control
    /// </summary>
    bool Focus();
    /// <summary>
    /// Invoked when the control has to preview the events
    /// </summary>
    Task PreviewEvents(TvConsoleEvents events);
    /// <summary>
    /// Invoked when the control has to handle the events
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    Task HandleEvents(TvConsoleEvents events);
    /// <summary>
    /// Return the associated metadata
    /// </summary>
    TvControlMetadata Metadata { get; }
}


/// <summary>
/// Extends ITvControl to support a specific state type.
/// </summary>
public interface ITvControl<TState> : ITvControl
{
    new TvComponent<TState> AsComponent();
    TvControlSetup<TState> Options { get; }
}


/// <summary>
/// Intermediate interface to offer fluent API.
/// [control].On().[event] will return the actions chain for the event.
/// [control].Raise() returns an ITvEventedControlEventRaiser to raise GainedFocus and LostFocus events.
/// </summary>
public interface ITvEventedControl : ITvControl
{
    ITvEventedControlActions On();
    ITvEventedControlEventRaiser Raise();
}

/// <summary>
/// Interface that all controls that raise GainedFocus and LostFocus events must implement.
/// This is needed because the framework will use this interface to raise these events when needed.
/// It's not intended to be used directly by the user.
/// </summary>
public interface ITvEventedControlEventRaiser
{
    Task GainedFocus();
    Task LostFocus();
}

public interface ITvEventedControlActions
{
    IActionsChain<ITvEventedControl> GainedFocus { get; }
    IActionsChain<ITvEventedControl> LostFocus { get; }
}