namespace Tvision2.Drawing.Tables;

public class TableRow
{
    private readonly List<TableCell> _cells = [];
    public void AddCell(string value)
    {
        var cell = new TableCell();
        cell.SetContent(value);
        _cells.Add(cell);
    }
}