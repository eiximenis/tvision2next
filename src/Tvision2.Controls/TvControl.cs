using System.Diagnostics;
using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Engine.Components;

namespace Tvision2.Controls;



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
    
    private readonly TvComponent<TState> _component;
    private readonly TvControlMetadata _metadata;

    protected TvComponent<TState> Component => _component;

    public TvControlMetadata Metadata => _metadata;
    
    public TvComponent AsComponent() => _component;

    public void MoveTo(TvPoint newPos) => _component.Viewport.MoveTo(newPos);
    protected TOptions Options { get; }
    
    protected internal TvControl(TvComponent<TState> component, TOptions options)
    {
        _metadata = new TvControlMetadata(this);
        component.Metadata.TagWith(TvControl.CONTROL_TAG, _metadata);
        _component = component; 
        Options = options;
    }

    public virtual Task PreviewEvents(TvConsoleEvents events) => Task.CompletedTask;

    public virtual Task HandleEvents(TvConsoleEvents events) => Task.CompletedTask;
    

    public bool Focus()
    {
        return _metadata.Focus();
    }
}

