using System.Diagnostics;
using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;
using Tvision2.Engine.Render;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.Button;


public class TvButtonOptions
{
    public bool AutoSize { get; set; } = false;
}

public interface IButtonActions
{
    IActionsChain<TvButton> Tapped { get; }
}

public class TvButton : TvControl<string, TvButtonOptions>, IButtonActions
{

    private readonly ActionsChain<TvButton> _tapped = new();

    public string Text
    {
        get => Component.State;
        set => Component.SetState(value);
    }

    public IButtonActions On() => this;

    IActionsChain<TvButton> IButtonActions.Tapped => _tapped;

    public TvButton(TvComponent<string> existingComponent, Action<TvButtonOptions>? optionsAction = null) :
        this(existingComponent, TvControl.RunOptionsAction(new TvButtonOptions(), optionsAction))
    {
    }
    public TvButton(TvComponent<string> component, TvButtonOptions options) : base(component, options, FocusPolicy.DirectFocusable)
    {
        Component.AddStyledDrawer(ButtonDrawer, "TvControls");
        Component.AddBehavior(AutoUpdateViewport);

    }


    public override Task PreviewEvents(TvConsoleEvents events)
    {
        Debug.Write("Preview event in Button :)");
        return Task.CompletedTask;
    }

    public override async Task HandleEvents(TvConsoleEvents events)
    {
        if (events.HasKeyboardEvents)
        {
            await _tapped.Invoke(this);
        }
    }

    private void AutoUpdateViewport(BehaviorContext<string> ctx)
    {
        if (!Options.AutoSize) return;

        var text = ctx.State;
        var bounds = ctx.Bounds;
        ctx.Resize(bounds.WithColumns(text.Length));
    }

    private DrawResult ButtonDrawer(StyledConsoleContext ctx, string text)
    {
        var state = Metadata.IsFocused ? "Focused" : "Normal"; 

        ctx.DrawStringAt(text, TvPoint.Zero, state);
        return DrawResult.Done;
    }
}