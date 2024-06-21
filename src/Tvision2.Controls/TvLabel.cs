using System.Reflection.Metadata;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Controls;

public class TvLabelOptions
{
    public bool AutoSize { get; set; } = true;
}

public class TvLabel : TvControl<string, TvLabelOptions>
{
    public string Text
    {
        get => Component.State;
        set => Component.SetState(value);
    }

    public TvLabel(TvComponent<string> existingComponent, Action<TvLabelOptions>? optionsAction = null) :
        this(existingComponent, TvControl.RunOptionsAction(new TvLabelOptions(), optionsAction))
    {
    }
    
    public TvLabel(TvComponent<string> existingComponent, TvLabelOptions options) : base(existingComponent, options) 
    {
        Component.AddDrawer(LabelDrawer);
        Component.AddBehavior(AutoUpdateViewport);
    }

    private void AutoUpdateViewport(BehaviorContext<string> ctx)
    {
        if (!Options.AutoSize) return;
        
        var text = ctx.State;
        var bounds = ctx.Bounds;
        ctx.Resize(bounds.WithColumns(text.Length));
    }
    
    private static void LabelDrawer(ConsoleContext ctx, string text)
    {
        ctx.DrawStringAt(text,TvPoint.Zero,  TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Yellow));
    }   
}