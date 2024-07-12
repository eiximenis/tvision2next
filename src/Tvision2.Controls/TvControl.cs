using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using Tvision2.Console.Events;
using Tvision2.Controls.Button;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls;



public static class TvControl
{
    internal const string CONTROL_TAG = "Tvision2::Control";
}

public class TvControl<TState> : ITvControl<TState>, ITvEventedControl, ITvControlActions
{
    private readonly TvControlSetup<TState> _controlOptions;
    public TvControlMetadata Metadata { get; }
    protected TvComponent<TState> Component { get; }
    TvControlSetup ITvControl.ControlOptions => _controlOptions;
    public ITvControlSetup<TState> ControlOptions => _controlOptions;
    TvComponent ITvControl.AsComponent() => AsComponent();
    public TvComponent<TState> AsComponent() => Component;

    private readonly ActionsChain<ITvEventedControl> _onGainedFocus = new();
    private readonly ActionsChain<ITvEventedControl> _onLostFocus = new();
    public ITvControlActions On() => this;
    IActionsChain<ITvEventedControl> ITvControlActions.GainedFocus => _onGainedFocus;
    IActionsChain<ITvEventedControl> ITvControlActions.LostFocus => _onLostFocus;

    public void MoveTo(TvPoint newPos) => Component.Viewport.MoveTo(newPos);

    public TvControl(TState initialState, Action<TvComponent<TState>>? config = null)
    {
        var cmp = new TvComponent<TState>(initialState, null);
        config?.Invoke(cmp);
        _controlOptions = new TvControlSetup<TState>();
        Component = cmp;
        Metadata = new TvControlMetadata(this);
        SetupComponent();
    }
    public TvControl(TvComponent<TState> component)
    {
        if (component.IsControl())
        {
            throw new InvalidOperationException("Component is already part of a a control");
        }

        _controlOptions = new TvControlSetup<TState>();
        Component = component;
        Metadata = new TvControlMetadata(this);
        SetupComponent();
    }

    private void SetupComponent()
    {
        Component.Metadata.TagWith(TvControl.CONTROL_TAG, Metadata);
        Component.AddBehavior(AutoUpdateViewport);
    }

    private void AutoUpdateViewport(BehaviorContext<TState> ctx)
    {
        if (_controlOptions is { AutoSize: true, SizeFunc: not null })
        {
            _controlOptions.SizeFunc(ctx);
        }
    }
    public Task PreviewEvents(TvConsoleEvents events) => _controlOptions.PreviewEvents?.Invoke(events) ?? Task.CompletedTask;
    public Task HandleEvents(TvConsoleEvents events) => _controlOptions.HandleEvents?.Invoke(events) ?? Task.CompletedTask;

    public bool Focus()
    {
        return Metadata.TryFocus();
    }

}

public class TvControlSetup
{
    public FocusPolicy FocusPolicy { get; set; } = FocusPolicy.NotFocusable;
    public Func<TvConsoleEvents, Task>? PreviewEvents { get; set; } = null;
    public Func<TvConsoleEvents, Task>? HandleEvents { get; set; } = null;
    public bool AutoSize { get; set; } = false;
}

public class TvControlSetup<T> : TvControlSetup, ITvControlSetup<T>
{

    public Action<BehaviorContext<T>>? SizeFunc { get; set; } = null;

    ITvControlSetup<T> ITvControlSetup<T>.WhenPreviewEventsDo(Func<TvConsoleEvents, Task> previewer)
    {
        PreviewEvents = previewer;
        return this;
    }

    ITvControlSetup<T> ITvControlSetup<T>.WhenHandleEventsDo(Func<TvConsoleEvents, Task> handler)
    {
        HandleEvents = handler;
        return this;
    }

    ITvControlSetup<T> ITvControlSetup<T>.WithAutoSize(Action<BehaviorContext<T>> sizeFunc)
    {
        SizeFunc = sizeFunc;
        return this;
    }
}

public interface ITvControlSetup<T>
{

    bool AutoSize { get; set; }

    FocusPolicy FocusPolicy { get; set; }

    ITvControlSetup<T> WhenPreviewEventsDo(Func<TvConsoleEvents, Task> previewer);

    ITvControlSetup<T> WhenHandleEventsDo(Func<TvConsoleEvents, Task> handler);

    ITvControlSetup<T> WithAutoSize(Action<BehaviorContext<T>> sizeFunc);

}
