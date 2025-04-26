using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Shapes;
using Tvision2.Drawing.Tables;

namespace Tvision2.Console;


public class Table
{
    public TvPoint TopLeft { get; }

    public TableDefinition Definition { get; }


    public TvBounds Bounds { get; set; }

    public Table (Box box, TableDefinition definition)
    {
        TopLeft = box.TopLeft;
        Bounds = box.Bounds;
        Definition = definition;
    }

    public Table(TvPoint topLeft, TableDefinition definition)
    {
        TopLeft = topLeft;
        Definition = definition;
    }


    public Box GetCellBox(int row, int column)
    {
        var previousHeight = 0;
        var rows = Definition.Rows;
        for (var ridx = 0; ridx < row; ridx++)
        {
            previousHeight += rows[ridx].ComputedHeight;
        }
        previousHeight += row + 1;          // Need to add all the top borders (1 for first cell, 2 for second,...)

        var height = rows[row].ComputedHeight;

        var previousWidth = 0;
        for (var cidx = 0; cidx < column; cidx++)
        {
            previousWidth += rows[row].CellAt(cidx).ComputedWidth;
        }
        previousWidth += column + 1;          // Need to add all the left borders (1 for first cell, 2 for second,...)

        var witdh = rows[row].CellAt(column).ComputedWidth;

        return new Box(TopLeft + TvPoint.FromXY(previousWidth, previousHeight), TvBounds.FromRowsAndCols(height, witdh ), BorderValue.None());

    }
}