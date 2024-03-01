using System.Diagnostics;
using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;
using Tvision2.Engine.Render;

namespace Tvision2.Controls.Extensions;


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

    private readonly ActionsChain<TvButton> _tapped;

    public string Text
    {
        get => _component.State;
        set => _component.SetState(value);
    }

    public IButtonActions On() => this;

    IActionsChain<TvButton> IButtonActions.Tapped => _tapped;

    public TvButton(TvComponent<string> existingComponent, Action<TvButtonOptions>? optionsAction = null) :
        this(existingComponent, TvControl.RunOptionsAction(new TvButtonOptions(), optionsAction))
    {
    }
    public TvButton(TvComponent<string> component, TvButtonOptions options) : base(component, options)
    {
        _component.AddDrawer(ButtonDrawer);
        _component.AddBehavior(AutoUpdateViewport);
        _tapped = new();
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

    private static void ButtonDrawer(ConsoleContext ctx, string text)
    {
        ctx.DrawStringAt(text, TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Yellow));
    }
}