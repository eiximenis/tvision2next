using Tvision2.Console.Events;
using Tvision2.Controls.Button;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;

namespace Tvision2.Controls;

public interface ITvControl
{
    TvComponent AsComponent();
    bool Focus();
    Task PreviewEvents(TvConsoleEvents events);
    Task HandleEvents(TvConsoleEvents events);

    TvControlMetadata Metadata { get; }
}

public interface ITvControl<TState> : ITvControl
{
    new TvComponent<TState> AsComponent();
    TvControlSetup<TState> Options { get; }
}


public interface ITvEventedControl : ITvControl
{
    ITvEventedControlActions On();
    ITvEventedControlEventRaiser Raise();
}

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