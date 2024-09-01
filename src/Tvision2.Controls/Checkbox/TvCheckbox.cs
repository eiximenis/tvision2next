using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Console.Events;
using Tvision2.Controls.Button;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;
using Tvision2.Engine.Render;
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

public interface ICheckboxActions : ITvEventedControlActions
{
    IActionsChain<TvCheckbox> CheckedChanged { get; }
}

public class TvCheckbox : TvEventedControl<TvCheckboxState>, ICheckboxActions
{
    private readonly ActionsChain<TvCheckbox> _checkedChanged = new();
    public string Text
    {
        get => Component.State.Text;
        set => Component.State.Text = value;
    }


    public new ICheckboxActions On() => this;

    public void On(Action<ICheckboxActions> onAction) => onAction.Invoke(this);

    public TvCheckbox(string text) : base(new TvCheckboxState() {Text = text, IsChecked = TvCheckboxChecked.Unchecked})
    {
        Options.AutoSize = true;
        Options.FocusPolicy = FocusPolicy.DirectFocusable;
        Options.WhenHandleEventsDo(OnHandleEvents).WithAutoSize(AutoUpdateViewport);
        Component.AddStyledDrawer(CheckboxDrawer, "TvControls");
    }
        
    private async Task OnHandleEvents(TvConsoleEvents events)
    {
        var evt = events.AcquireFirstKeyboard(e => e.AsConsoleKeyInfo().Key == ConsoleKey.Spacebar, autoHandle: true);
        if (evt is null)
        {
            return;
        }
        var state = Component.State;
        state.IsChecked = state.IsChecked switch
        {
            TvCheckboxChecked.Unchecked => TvCheckboxChecked.PartiallyChecked,
            TvCheckboxChecked.PartiallyChecked => TvCheckboxChecked.Checked,
            _ => TvCheckboxChecked.Unchecked
        };

        await _checkedChanged.Invoke(this);
    }

    private DrawResult CheckboxDrawer(StyledConsoleContext ctx, TvCheckboxState checkState)
    {
        var state = Metadata.IsFocused ? "Focused" : "Normal";
        ctx.Fill(state);
        var inner = checkState.IsChecked switch
        {
            TvCheckboxChecked.Checked => "x",
            TvCheckboxChecked.PartiallyChecked => "P",
            _ => " "
        };
        ctx.DrawStringAt($"[{inner}]", TvPoint.Zero);
        ctx.DrawStringAt(checkState.Text, TextPosition.CenterHorizontally(margin: Margin.LeftMargin(3)));
        return DrawResult.Done;
    }

    private void AutoUpdateViewport(BehaviorContext<TvCheckboxState> ctx)
    {
        var bounds = ctx.Bounds;
        ctx.Resize(TvBounds.FromRowsAndCols(1, ctx.State.Text.Length + 10));
    }

    IActionsChain<TvCheckbox> ICheckboxActions.CheckedChanged => _checkedChanged;
}
