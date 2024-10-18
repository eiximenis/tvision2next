namespace Tvision2.Drawing.Tables;

public class TableRow
{
    public RowHeight Height { get; }

    internal int ComputedHeight { get; set; }

    public TableRow(RowHeight height)
    {
        Height = height;
    }

    private readonly List<TableCell> _cells = [];
    public void AddCell(ColumnWidth width)
    {
        var cell = new TableCell(width);
        _cells.Add(cell);
    }
}