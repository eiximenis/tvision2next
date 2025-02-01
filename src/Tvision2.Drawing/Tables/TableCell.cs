namespace Tvision2.Drawing.Tables;

public class TableCell
{
    public ColumnWidth Width { get; }

    internal int ComputedWidth { get; set; }
    public TableCell(ColumnWidth width)
    {
        Width = width;
    }
}