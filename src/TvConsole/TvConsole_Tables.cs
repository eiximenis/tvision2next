using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Tables;

namespace Tvision2.Console;
partial class TvConsole
{
    public static void Draw(Table table, TvPoint pos)
    {
        // Draw outer border
        Border.Draw(ConsoleDrawer, table.Border, pos, table.Bounds, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.Red)); 

        foreach (var row in table.Rows)
        {
            row.AddCell("xxxx");
        }
    }
}
