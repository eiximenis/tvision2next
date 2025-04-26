using System.Runtime.InteropServices;
using Tvision2.Core;

namespace Tvision2.Drawing.Tables;

public class TableRow
{
    public RowHeight Height { get; }

    public int ComputedHeight { get; internal set; }

    public TableRow(RowHeight height)
    {
        Height = height;
    }

    private readonly List<TableCell> _cells = [];

    public IEnumerable<TableCell> Cells => _cells;

    public int CellsCount => _cells.Count;

    public TableRow AddCell(ColumnWidth width)
    {
        var cell = new TableCell(width);
        _cells.Add(cell);
        return this;
    }

    public TableRow AddCells(params ColumnWidth[] widths)
    {
        foreach (var width in widths)
        {
            var cell = new TableCell(width);
            _cells.Add(cell);
        }

        return this;
    }

    public TableRow AddCells(int count)
    {
        for (var idx = 0; idx < count; idx++)
        {
            var cell = new TableCell(ColumnWidth.Relative(1));
            _cells.Add(cell);
        }

        return this;
    }

    public void  CalculateColumnWidths(int totalWidth)
    {
        var innerWidth = totalWidth - 2;
        var innerBordersWitdh = _cells.Count - 1;
        var cellsAvailableWidth = innerWidth - innerBordersWitdh;

        var totalFixedWidth = _cells.Where(c => c.Width.Type == ColumnWidthType.Fixed).Sum(c => c.Width.Value);
        var totalRelativeWidth = _cells.Where(c => c.Width.Type == ColumnWidthType.Relative).Sum(c => c.Width.Value);
        var relativeUnit = totalRelativeWidth > 0  ? (cellsAvailableWidth - totalFixedWidth) / totalRelativeWidth : 0;

        var ocuppedWitdh = 0;
        for (var cellIdx = 0; cellIdx < _cells.Count -1 ; cellIdx++)
        {
            var cell = _cells[cellIdx];
            cell.ComputedWidth = cell.Width.Type == ColumnWidthType.Fixed ? cell.Width.Value : cell.Width.Value * relativeUnit;
            ocuppedWitdh += cell.ComputedWidth;
        }

        var lastCell = _cells[^1];
        lastCell.ComputedWidth = cellsAvailableWidth - ocuppedWitdh;
    }

    public TableCell CellAt(int column) => _cells[column];
}