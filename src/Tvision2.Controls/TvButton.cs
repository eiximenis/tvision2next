using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Controls;


public class TvButtonOptions
{
    public bool AutoSize { get; set; } = false;
} 

public class TvButton : TvControl<string, TvButtonOptions>
{
    
    public string Text
    {
        get => _component.State;
        set => _component.SetState(value);
    }
    
    public TvButton(TvComponent<string> existingComponent, Action<TvButtonOptions>? optionsAction = null) :
        this(existingComponent, TvControl.RunOptionsAction(new TvButtonOptions(), optionsAction))
    {
    }
    public TvButton(TvComponent<string> component, TvButtonOptions options) : base(component, options)
    {
        _component.AddDrawer(ButtonDrawer);
        _component.AddBehavior(AutoUpdateViewport);
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
        ctx.DrawStringAt(text,TvPoint.Zero,  TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Yellow));
    }
}