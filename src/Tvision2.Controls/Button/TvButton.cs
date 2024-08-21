using System.Diagnostics;
using System.Globalization;
using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;
using Tvision2.Engine.Render;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.Button;

public interface IButtonActions : ITvEventedControlActions
{
    IActionsChain<TvButton> Tapped { get; }
}

public class TvButton : TvEventedControl<string>, IButtonActions
{

    private readonly ActionsChain<TvButton> _tapped = new();

    public string Text
    {
        get => Component.State;
        set => Component.SetState(value);
    }

    
    public new IButtonActions On() => this;

    IActionsChain<TvButton> IButtonActions.Tapped => _tapped;

    public TvButton(string text) : base(text)
    {
        Options.AutoSize = true;
        Options.FocusPolicy = FocusPolicy.DirectFocusable;
        Options.WhenPreviewEventsDo(PreviewEvents).WhenHandleEventsDo(HandleEvents).WithAutoSize(AutoUpdateViewport);
        Component.AddStyledDrawer(ButtonDrawer, "TvControls");
    }

    public TvButton(TvComponent<string> component) : base(component)
    {
        Options.AutoSize = true;
        Options.FocusPolicy = FocusPolicy.DirectFocusable;
        Options.WhenPreviewEventsDo(PreviewEvents).WhenHandleEventsDo(HandleEvents).WithAutoSize(AutoUpdateViewport);
        Component.AddStyledDrawer(ButtonDrawer, "TvControls");
    }

    public Task PreviewEvents(TvConsoleEvents events)
    {
        Debug.Write($"Preview {events.Count} events in Button :)");
        return Task.CompletedTask;
    }

    public async Task HandleEvents(TvConsoleEvents events)
    {
        if (events.HasKeyboardEvents)
        {
            await _tapped.Invoke(this);
        }
    }

    private void AutoUpdateViewport(BehaviorContext<string> ctx)
    {
        var text = ctx.State;
        var bounds = ctx.Bounds;
        ctx.Resize(TvBounds.FromRowsAndCols(1, text.Length + 2));
    }

    private DrawResult ButtonDrawer(StyledConsoleContext ctx, string text)
    {
        var state = Metadata.IsFocused ? "Focused" : "Normal";
        ctx.Fill(state);
        ctx.DrawStringAt(text, TvPoint.Zero, state);
        return DrawResult.Done;
    }
}