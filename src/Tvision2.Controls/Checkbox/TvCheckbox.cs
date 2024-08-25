using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.Checkbox;


public enum TvCheckboxChecked
{
    Unchecked,
    PartiallyChecked,
    Checked,
}
public class TvCheckboxState
{
    public string Text { get; internal set; }
    public TvCheckboxChecked IsChecked { get; internal set; }
}
public class TvCheckbox : TvEventedControl<TvCheckboxState>
{
    public string Text
    {
        get => Component.State.Text;
        set => Component.State.Text = value;
    }

    public TvCheckbox(string text) : base(new TvCheckboxState() {Text = text, IsChecked = TvCheckboxChecked.Unchecked})
    {
        Options.AutoSize = true;
        Options.FocusPolicy = FocusPolicy.DirectFocusable;
        Options.WhenPreviewEventsDo(PreviewEvents).WhenHandleEventsDo(HandleEvents).WithAutoSize(AutoUpdateViewport);
        Component.AddStyledDrawer(CheckboxDrawer, "TvControls");
    }

    private DrawResult CheckboxDrawer(StyledConsoleContext ctx, TvCheckboxState checkState)
    {
        var state = Metadata.IsFocused ? "Focused" : "Normal";
        ctx.Fill(state);
        ctx.DrawStringAt("[ ]", TvPoint.Zero);
        return DrawResult.Done;
    }

    private void AutoUpdateViewport(BehaviorContext<TvCheckboxState> ctx)
    {
        var bounds = ctx.Bounds;
        ctx.Resize(TvBounds.FromRowsAndCols(1, ctx.State.Text.Length + 3));
    }
}
