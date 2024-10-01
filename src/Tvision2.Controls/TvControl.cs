using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

/// <summary>
/// (Possible non-mandatory) base class for controls.
/// This class contains basic implementation of ITvControl[T] interface and ensures
/// the control is correctly tagged with the TvControl.CONTROL_TAG tag.
/// </summary>
public class TvControl<TState> : ITvControl<TState>
{
    public TvControlMetadata Metadata { get; }
    protected TvComponent<TState> Component { get; }
    TvComponent ITvControl.AsComponent() => AsComponent();
    public TvComponent<TState> AsComponent() => Component;

    public void MoveTo(TvPoint newPos) => Component.Viewport.MoveTo(newPos);

    public void Resize(TvBounds newBounds) => Component.Viewport.Resize(newBounds);

    public TvControlSetup<TState> Options { get; }

    public TvControl(TState initialState, Action<TvComponent<TState>>? config = null)
    {
        var cmp = new TvComponent<TState>(initialState, null);
        config?.Invoke(cmp);
        Options = new TvControlSetup<TState>();
        Component = cmp;
        Metadata = new TvControlMetadata(this, Options);
        SetupComponent();
    }
    public TvControl(TvComponent<TState> component)
    {
        if (component.IsControl())
        {
            throw new InvalidOperationException("Component is already part of a a control");
        }

        Options = new TvControlSetup<TState>();
        Component = component;
        Metadata = new TvControlMetadata(this, Options);
        SetupComponent();
    }

    private void SetupComponent()
    {
        Component.Metadata.TagWith(TvControl.CONTROL_TAG, Metadata);
        Component.AddBehavior(AutoUpdateViewport);
    }

    private void AutoUpdateViewport(BehaviorContext<TState> ctx)
    {
        if (Options is { AutoSize: true, SizeFunc: not null })
        {
            Options.SizeFunc(ctx);
        }
    }
    public Task PreviewEvents(TvConsoleEvents events) => Options.PreviewEvents?.Invoke(events) ?? Task.CompletedTask;
    public Task HandleEvents(TvConsoleEvents events) => Options.HandleEvents?.Invoke(events) ?? Task.CompletedTask;

    public bool Focus()
    {   
        return Metadata.TryFocus();
    }
}

/// <summary>
/// (Possible non-mandatory) base class for controls that raise the basic events GainedFocus and LostFocus
/// It offers a basic implementation to subscribe to these events (through ITvEventedControlActions) and to raise them (through ITvEventedControlEventRaiser)
/// However, framework never assumes a control is instance of this class, so it is not mandatory to use it.
/// </summary>
/// <typeparam name="TState"></typeparam>
public class TvEventedControl<TState> : TvControl<TState>, ITvEventedControl, ITvEventedControlActions, ITvEventedControlEventRaiser
{

    private readonly ActionsChain<ITvEventedControl> _onGainedFocus = new();
    private readonly ActionsChain<ITvEventedControl> _onLostFocus = new();
    public ITvEventedControlActions On() => this;
    IActionsChain<ITvEventedControl> ITvEventedControlActions.GainedFocus => _onGainedFocus;
    IActionsChain<ITvEventedControl> ITvEventedControlActions.LostFocus => _onLostFocus;
    Task ITvEventedControlEventRaiser.GainedFocus() => _onGainedFocus.Invoke(this);
    Task ITvEventedControlEventRaiser.LostFocus() => _onLostFocus.Invoke(this);

    ITvEventedControlEventRaiser ITvEventedControl.Raise() => this;

    public TvEventedControl(TState initialState, Action<TvComponent<TState>>? config = null) : base(initialState, config)
    {
    }
    public TvEventedControl(TvComponent<TState> component) : base(component)
    {
    }

}

public class TvControlSetup 
{
    public FocusPolicy FocusPolicy { get; set; } = FocusPolicy.NotFocusable;

    public Func<TvConsoleEvents, Task>? PreviewEvents { get; set; } = null;
    public Func<TvConsoleEvents, Task>? HandleEvents { get; set; } = null;
    public bool AutoSize { get; set; } = false;
}

public class TvControlSetup<T> : TvControlSetup
{

    public Action<BehaviorContext<T>>? SizeFunc { get; set; } = null;

    public TvControlSetup<T> WhenPreviewEventsDo(Func<TvConsoleEvents, Task> previewer)
    {
        PreviewEvents = previewer;
        return this;
    }

    public TvControlSetup<T> WhenHandleEventsDo(Func<TvConsoleEvents, Task> handler)
    {
        HandleEvents = handler;
        return this;
    }


    public TvControlSetup<T> WithoutAutoSize()
    {
        AutoSize = false;
        return this;
    }
    public TvControlSetup<T> WithAutoSize(Action<BehaviorContext<T>> sizeFunc)
    {
        SizeFunc = sizeFunc;
        return this;
    }
}

