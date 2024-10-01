using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;
using Tvision2.Layouts;

namespace Tvision2.Controls.Layout.Grid;
 public class TvGrid : TvEventedControl<GridState>
{
    private readonly GridContainer _gridContainer;
    public TvGrid(GridState state) : base(state)
    {
        _gridContainer = new GridContainer(AsComponent().AsContainer(), state);
        Options.AutoSize = false;
        Component.AddDrawer(GridDrawer);
    }

    private void GridDrawer(ConsoleContext ctx, GridState state)
    {
        // Draw outside Border
        var drawer = ctx.GetConsoleDrawer();

        Border.Draw(drawer, BorderValue.Single(), TvPoint.Zero, ctx.Viewzone.Bounds, TvColorsPair.FromForegroundAndBackground(TvColor.LightGreen, TvColor.Blue));
    }
}
