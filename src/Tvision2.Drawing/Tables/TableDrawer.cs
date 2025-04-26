using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;

namespace Tvision2.Drawing.Tables
{

    public class TableDefinition
    {
        private readonly List<TableRow> _rows = [];

        public ReadOnlySpan<TableRow> Rows => CollectionsMarshal.AsSpan(_rows);

        public BorderValue Border { get;  }

        public int RowsCount => _rows.Count;


        public TableDefinition() : this(BorderValue.Single()) { }

        public TableDefinition(BorderValue border)
        {
            Border = border;
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

    }

    public static class TableDrawer
    {
        public static void Draw<TD>(TD drawer, TableDefinition table, in TvPoint topLeft, in TvBounds bounds) where TD : IConsoleDrawer
        {
            // Draw outer border
            Borders.BorderDrawer.Draw(drawer, table.Border, topLeft, bounds, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.Red));
            CalculateCellsBounds(table, in bounds);
            var idx = 0;
            var currentPos = topLeft + TvPoint.FromXY(0, 1);
            var rows = table.Rows;
            for (var rowIdx = 0; rowIdx < table.RowsCount; rowIdx++)
            {
                var row = rows[rowIdx];
                var cellPos = currentPos;
                for (var cidx = 0; cidx < row.CellsCount - 1; cidx++)
                {
                    var cell = row.CellAt(cidx);
                    cellPos = cellPos + TvPoint.FromXY(cell.ComputedWidth + 1, 0);
                    var vline = new VerticalLine(LineType.Single, BorderType.None);
                    vline.Draw(drawer, cellPos, row.ComputedHeight, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.Red));
                }

                if (rowIdx == table.RowsCount - 1) continue;        // We don't want to draw the last horizontal line (because outer borders have already been drawn)
                idx++;
                currentPos = currentPos with { Y = currentPos.Y + row.ComputedHeight };
                var line = new HorizontalLine(LineType.Single, BorderType.None);
                line.Draw(drawer, currentPos + TvPoint.FromXY(1, 0), bounds.Width - 2, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.Red));
                currentPos = currentPos with { Y = currentPos.Y + 1 };
            }
        }

        public static void CalculateCellsBounds(TableDefinition table, in TvBounds bounds)
        {
            var height = bounds.Height;
            var innerHeight = height - 2;
            var innerBordersHeight = table.RowsCount - 1;
            var cellsAvailableHeight = innerHeight - innerBordersHeight;
            var rows = table.Rows;

            var totalFixedHeight = 0;
            var totalRelativeHeight = 0;
            for (var ridx = 0; ridx < rows.Length; ridx++)
            {
                var row = rows[ridx];
                if (row.Height.Type == RowHeightType.Fixed)
                {
                    totalFixedHeight += row.Height.Value;
                }
                else
                {
                    totalRelativeHeight += row.Height.Value;
                }
            }

            var relativeUnit = totalRelativeHeight > 0 ? (cellsAvailableHeight) / totalRelativeHeight : 0;
            var ocuppedHeight = 0;
            for (var rowIdx = 0; rowIdx < table.RowsCount - 1; rowIdx++)
            {
                var row = rows[rowIdx];
                row.ComputedHeight = row.Height.Type == RowHeightType.Fixed ? row.Height.Value : row.Height.Value * relativeUnit;
                ocuppedHeight += row.ComputedHeight;
                row.CalculateColumnWidths(bounds.Width);
            }
            var lastRow = rows[^1];
            lastRow.ComputedHeight = cellsAvailableHeight - ocuppedHeight;
            lastRow.CalculateColumnWidths(bounds.Width);
        }
    }
}
