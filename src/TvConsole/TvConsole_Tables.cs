using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Tables;

namespace Tvision2.Console;
partial class TvConsole
{
    /// <summary>
    /// Draw a row of values inside a one-row-table
    /// </summary>
    public static void Draw(IEnumerable<string> values)
    {
        var table = new Table(border: BorderValue.None());
        var row = table.AddRow();
        foreach (var value in values)
        {
            row.AddCell(value);
        }
        Draw(table);

    }

    public static void Draw(Table table)
    {
        foreach (var row in table.Rows)
        {
            // TODO: Continue here :)
        }
    }
}
