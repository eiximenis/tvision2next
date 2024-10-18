namespace Tvision2.Drawing.Tables;

class TableCell
{
    public ColumnWidth Width { get; }
    public TableCell(ColumnWidth width)
    {
        Width = width;
    }
}