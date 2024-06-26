using Tvision2.Console.Events;
using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public interface ITvControl
{
    TvComponent AsComponent();
    bool Focus();
    Task PreviewEvents(TvConsoleEvents events);
    Task HandleEvents(TvConsoleEvents events);

    TvControlMetadata Metadata { get; }
}

public interface ITvControl<TState, TOptions>  : ITvControl
{
}
