using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Tables;
using Tvision2.Engine.Render;

using _Drawer = Tvision2.Drawing.Tables.TableDrawer;

namespace Tvision2.Controls.Panels;

public class TvTableState
{
    public TvTableState()
    {
    }
}
public class TvTable : TvEventedControl<TvTableState>
{
    public TvTable() : base(new TvTableState())
    {
        Options.AutoSize = false;
        Component.AddDrawer(TableDrawer);

    }

    private void TableDrawer(ConsoleContext ctx, TvTableState state)
    {
        var drawer = ctx.GetConsoleDrawer();

        var table = new TableDefinition(BorderValue.Single());
        table.AddRow(RowHeight.Fixed(1)).AddCells(2);
        table.AddRow(RowHeight.Fixed(1)).AddCells(4);
        _Drawer.Draw(ctx.GetConsoleDrawer(), table, TvPoint.Zero, ctx.Viewzone.Bounds);
        
    }
}
