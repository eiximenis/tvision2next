using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Controls;

public class TvLabelOptions
{
    
}

public class TvLabel
{
    private TvComponent<string> _component;

    public string Text
    {
        get => _component.State;
        set => _component.SetState(value);
    }

    private TvLabelOptions Options { get;  }

    public TvComponent AsComponent() => _component;

    public TvLabel(Action<TvLabelOptions>? optionsSetup = null, TvComponent<string>? existingComponent = null)
    {
        Options = new TvLabelOptions();
        optionsSetup?.Invoke(Options);
        var text = "TvLabel";
        var viewport = new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(1, text.Length));
        _component = existingComponent ?? new TvComponent<string>(text, viewport);
        _component.AddDrawer(LabelDrawer);
        Text = text;
    }

    private void LabelDrawer(ConsoleContext ctx)
    {
        ctx.DrawStringAt(Text,TvPoint.Zero,  TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Black));
    }   
}