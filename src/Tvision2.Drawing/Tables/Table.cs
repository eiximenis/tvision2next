using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Shapes;

namespace Tvision2.Drawing.Tables;



public class Table
{
    private readonly BorderValue _border;
    private readonly List<TableRow> _rows = [];

    public BorderValue Border => _border;

    public TvBounds Bounds { get; set; }

    public ReadOnlySpan<TableRow> Rows => CollectionsMarshal.AsSpan(_rows);

    public Table(BorderValue border)
    {
        _border = border;
    }
    public TableRow AddRow(RowHeight rowHeight)
    {
        var row = new TableRow(rowHeight);
        _rows.Add(row);
        return row;
    }

    public TableRowSet AddRows(int rowCount)
    {
        var currentRows = _rows.Count;
        for (var idx = 0; idx < rowCount; idx++)
        {
            _rows.Add(new TableRow(RowHeight.Relative(1)));
        }

        return new TableRowSet(CollectionsMarshal.AsSpan(_rows).Slice(currentRows, rowCount));
    }

    private void CalculateCellsBounds()
    {
        var height = Bounds.Height;
        var innerHeight = height - 2;
        var innerBordersHeight = _rows.Count - 1;
        var cellsAvailableHeight = innerHeight - innerBordersHeight;

        var totalFixedHeight = _rows.Where(r => r.Height.Type == RowHeightType.Fixed).Sum(r => r.Height.Value);
        var totalRelativeHeight = _rows.Where(r => r.Height.Type == RowHeightType.Relative).Sum(r => r.Height.Value);
        var relativeUnit = totalRelativeHeight > 0 ? (cellsAvailableHeight) / totalRelativeHeight : 0;
        var ocuppedHeight = 0;
        for (var rowIdx = 0; rowIdx < _rows.Count - 1; rowIdx++)
        {
            var row = _rows[rowIdx];
            row.ComputedHeight = row.Height.Type == RowHeightType.Fixed ? row.Height.Value : row.Height.Value * relativeUnit;
            ocuppedHeight += row.ComputedHeight;
            row.CalculateColumnWidths(Bounds.Width);
        }
        var lastRow = _rows[^1];
        lastRow.ComputedHeight = cellsAvailableHeight - ocuppedHeight;
        lastRow.CalculateColumnWidths(Bounds.Width);
    }



    public void Draw<TD>(TD drawer, TvPoint pos) where TD : IConsoleDrawer
    {
        // Draw outer border
        Borders.Border.Draw(drawer, Border, pos, Bounds, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.Red));
        CalculateCellsBounds();
        var idx = 0;
        var currentPos = pos + TvPoint.FromXY(0, 1);
        for (var rowIdx = 0; rowIdx < _rows.Count; rowIdx++)
        {
            var row = _rows[rowIdx];
            var cellPos = currentPos;
            for (var cidx = 0; cidx < row.CellsCount - 1; cidx++)
            {
                var cell = row.CellAt(cidx);
                cellPos = cellPos + TvPoint.FromXY(cell.ComputedWidth + 1, 0);
                var vline = new VerticalLine(LineType.Single, BorderType.None);
                vline.Draw(drawer, cellPos,  row.ComputedHeight, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.Red));
            }

            if (rowIdx == _rows.Count - 1) continue;        // We don't want to draw the last horizontal line (because outer borders have already been drawn)
            idx++;
            currentPos = currentPos with { Y = currentPos.Y + row.ComputedHeight };
            var line = new HorizontalLine(LineType.Single, BorderType.None);
            line.Draw(drawer, currentPos + TvPoint.FromXY(1,0), Bounds.Width - 2, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.Red));
            currentPos = currentPos with { Y = currentPos.Y + 1 };
        }
    }

    public Box GetCellBox(TvPoint topLeft, int row, int column)
    {
        var previousHeight = 0;
        for (var ridx = 0; ridx < row; ridx++)
        {
            previousHeight += _rows[ridx].ComputedHeight;
        }
        previousHeight += row + 1;          // Need to add all the top borders (1 for first cell, 2 for second,...)

        var height = _rows[row].ComputedHeight;

        var previousWidth = 0;
        for (var cidx = 0; cidx < column; cidx++)
        {
            previousWidth += _rows[row].CellAt(cidx).ComputedWidth;
        }
        previousWidth += column + 1;          // Need to add all the left borders (1 for first cell, 2 for second,...)

        var witdh = _rows[row].CellAt(column).ComputedWidth;

        return new Box(topLeft + TvPoint.FromXY(previousWidth, previousHeight), TvBounds.FromRowsAndCols(height, witdh ), BorderValue.None());

    }
}