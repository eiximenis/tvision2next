using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;
using Tvision2.Engine.Components;
using Tvision2.Engine.Drawing;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.List;

public class ListState
{
    private readonly List<string> _items = [];
    private ListState() {}
    public static ListState FromStrings(IEnumerable<string> data)
    {
        var self = new ListState();
        self._items.AddRange(data);
        return self;
    }
}
public class TvList : TvEventedControl<ListState>
{
    public TvList(IEnumerable<string> data) : base(ListState.FromStrings(data))
    {
        SetupComponent();
    }

    public TvList(TvComponent<ListState> existingComponent) : base(existingComponent)
    {
        SetupComponent();
    }

    private void SetupComponent()
    {
        Options.AutoSize = false;
        Component.AddStyledDrawer(ListDrawer, "TvControls");
        // Component.AddBehavior(AutoUpdateViewport);
    }

    private DrawResult ListDrawer(StyledConsoleContext ctx, ListState state)
    {
        var drawer = ctx.GetConsoleDrawer();
        Border.Draw(drawer, BorderValue.Single(), TvPoint.Zero, ctx.Viewzone.Bounds, TvColorsPair.FromForegroundAndBackground(TvColor.Green, TvColor.Blue));
        var rows = ctx.Viewzone.Bounds.Height - 2;         
       

        return DrawResult.Done;
    }
}
