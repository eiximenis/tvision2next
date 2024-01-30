using System.Reflection.Metadata;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Controls;

public class TvLabelOptions
{
    
}

public class TvLabel : TvControl<string, TvLabelOptions>
{
    public string Text
    {
        get => _component.State;
        set => _component.SetState(value);
    }

    public TvLabel(TvComponent<string> existingComponent, TvLabelOptions options) : base(existingComponent, options) 
    {
        var text = "TvLabel";
        _component.AddDrawer(LabelDrawer);
        Text = text;
    }
    public TvLabel(TvComponent<string> existingComponent, Action<TvLabelOptions>? optionsAction = null) :
        this(existingComponent, TvControl.RunOptionsAction(new TvLabelOptions(), optionsAction))
    {
    }

    private void LabelDrawer(ConsoleContext ctx)
    {
        ctx.DrawStringAt(Text,TvPoint.Zero,  TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Black));
    }   
}