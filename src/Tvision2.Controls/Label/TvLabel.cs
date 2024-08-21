using System.Reflection.Metadata;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.Label;

public class TvLabel : TvEventedControl<string>
{
    public string Text
    {
        get => Component.State;
        set => Component.SetState(value);
    }


    public TvLabel(string initialText = "") : base(initialText)
    {
        SetupComponent();
    }

    public TvLabel(TvComponent<string> existingComponent) : base(existingComponent)
    {
        SetupComponent();
    }

    private void SetupComponent()
    {
        Options.AutoSize = true;
        Component.AddStyledDrawer(LabelDrawer, "TvControls");
        Component.AddBehavior(AutoUpdateViewport);
    }

    private void AutoUpdateViewport(BehaviorContext<string> ctx)
    {
        if (!Options.AutoSize) return;

        var text = ctx.State;
        var bounds = ctx.Bounds;
        ctx.Resize(TvBounds.FromRowsAndCols(1, text.Length));
    }

    private static DrawResult LabelDrawer(StyledConsoleContext ctx, string text)
    {
        ctx.DrawStringAt(text, TvPoint.Zero);
        return DrawResult.Done;
    }
}