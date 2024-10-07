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
 public class TvGrid : TvEventedControl<GridDefinition>
{
    private readonly GridContainer _gridContainer;
    public TvGrid(GridDefinition definition) : base(definition)
    {
        _gridContainer = new GridContainer(AsComponent().AsContainer(), definition);
        Options.AutoSize = false;
        Component.AddDrawer(GridDrawer);
    }

    private void GridDrawer(ConsoleContext ctx, GridDefinition definition)
    {
        // Draw outside Border
        var drawer = ctx.GetConsoleDrawer();

        Border.Draw(drawer, BorderValue.Single(), TvPoint.Zero, ctx.Viewzone.Bounds, TvColorsPair.FromForegroundAndBackground(TvColor.LightGreen, TvColor.Blue));
    }
}
