using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Tables;
using Tvision2.Drawing.Text;

namespace Tvision2.Console;
partial class TvConsole
{
    public static void Draw(Table table)
    {
        TableDrawer.Draw(ConsoleDrawer, table.Definition, table.TopLeft, table.Bounds);
    }
}
