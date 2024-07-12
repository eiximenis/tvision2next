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
    TvControlSetup ControlOptions { get; }
}

public interface ITvControl<TState> : ITvControl
{
    new TvComponent<TState> AsComponent();
    new ITvControlSetup<TState> ControlOptions { get; }
}


public interface ITvEventedControl : ITvControl
{
    ITvControlActions On();
}

public interface ITvControlActions
{
    IActionsChain<ITvEventedControl> GainedFocus { get; }
    IActionsChain<ITvEventedControl> LostFocus { get; }
}

