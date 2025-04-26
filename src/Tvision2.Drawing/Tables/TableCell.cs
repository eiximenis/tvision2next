namespace Tvision2.Drawing.Tables;

public class TableCell
{
    public ColumnWidth Width { get; }

    public int  ComputedWidth { get; internal set; }
    public TableCell(ColumnWidth width)
    {
        Width = width;
    }
}