using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public interface ITvControl
{
    TvComponent AsComponent();
    bool Focus();
    Task PreviewEvents(TvConsoleEvents events) => Task.CompletedTask;
    Task HandleEvents(TvConsoleEvents events) => Task.CompletedTask;
}

public interface ITvControl<TState, TOptions> : ITvControl
{
}

public static class TvControl
{
    internal const string CONTROL_TAG = "Tvision2::Control";
     

    public static TvControl<TState, TOptions> Wrap<TState, TOptions>(TvComponent<TState> componentToWrap, TOptions options) =>
        new TvControl<TState, TOptions>(componentToWrap, options);

    public static IControlFactory Factory { get; } = new TvControlFactory();
    
    public static TOptions RunOptionsAction<TOptions>(TOptions options, Action<TOptions>? optionsAction )
    {
        if (optionsAction is not null)
        {
            optionsAction.Invoke(options);
        }
        return options;
    }
}


public class TvControl<TState, TOptions> : ITvControl<TState, TOptions>
{
    
    protected readonly TvComponent<TState> _component;
    
    public TvComponent AsComponent() => _component;

    public void MoveTo(TvPoint newPos) => _component.Viewport.MoveTo(newPos);
    protected TOptions Options { get; }
    
    protected internal TvControl(TvComponent<TState> component, TOptions options)
    {
        component.Metadata.TagWith(TvControl.CONTROL_TAG, new TvControlMetadata(this));
        _component = component;
        Options = options;
    }

    public bool Focus()
    {
        return true;
    }
}

