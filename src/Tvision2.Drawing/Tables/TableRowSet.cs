namespace Tvision2.Drawing.Tables;

public ref struct TableRowSet
{
    private readonly ReadOnlySpan<TableRow> _rows;
    public TableRowSet(ReadOnlySpan<TableRow> rows)
    {
        _rows = rows;
    }
    public TableRowSet AddCell(ColumnWidth width)
    {
        foreach (var row in _rows)
        {
            row.AddCell(width);
        }

        return this;
    }

    public TableRowSet AddCells(params ColumnWidth[] widths)
    {
        foreach (var row in _rows)
        {
            row.AddCells(widths);
        }

        return this;
    }

    public TableRowSet AddCells(int count)
    {
        foreach (var row in _rows)
        {
            row.AddCells(count);
        }
        return this;
    }

}