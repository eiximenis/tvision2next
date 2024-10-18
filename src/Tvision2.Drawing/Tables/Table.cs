using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;

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

    public void AddRows(int rowCount)
    {
        for (var idx=0; idx < rowCount; idx++)
        {
            _rows.Add(new TableRow(RowHeight.Relative(1)));
        }
    }

    private void CalculateCellsBounds()
    {
        var height = Bounds.Height;
        var bordersHeight = _rows.Count - 1;
        var totalFixedHeight = _rows.Where(r => r.Height.Type == RowHeightType.Fixed).Sum(r => r.Height.Value);
        var totalRelativeHeight = _rows.Where(r => r.Height.Type == RowHeightType.Relative).Sum(r => r.Height.Value);
        var relativeUnit = (height - bordersHeight - totalFixedHeight) / totalRelativeHeight;
        foreach (var row in _rows)
        {
            row.ComputedHeight = row.Height.Type == RowHeightType.Fixed ? row.Height.Value : row.Height.Value * relativeUnit;
        }
    }

    public void Draw<TD>(TD drawer, TvPoint pos) where TD : IConsoleDrawer
    {
        // Draw outer border
        Borders.Border.Draw(drawer, Border, pos, Bounds, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.Red));
        CalculateCellsBounds();
        var idx = 0;
        var currentPos = pos + TvPoint.FromXY(1, 1);
        foreach (var row in _rows)
        {
            idx++;
            drawer.DrawStringAt($"row {idx}", currentPos , TvColorsPair.FromForegroundAndBackground(TvColor.Red, TvColor.White));
            currentPos = currentPos with { Y = currentPos.Y + row.ComputedHeight };
            var line = new Line(LineType.Dotted, BorderType.None);
            line.Draw(drawer, currentPos, Bounds.Width - 2, TvColorsPair.FromForegroundAndBackground(TvColor.Green, TvColor.White));
            currentPos = currentPos with { Y = currentPos.Y + 1 };
        }
    }
}