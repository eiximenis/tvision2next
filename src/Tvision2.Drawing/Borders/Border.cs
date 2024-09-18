using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;

namespace Tvision2.Drawing.Borders;
public static class Border
{
    /// <summary>
    /// Draws a border in the specified location with the specified bounds and colors
    /// Notes:
    /// 1. When called from TvConsole project, the location is relative to the console itself.
    /// 2. When called from a Tvision2 drawer, the location is relative to the ConsoleContext location (use TvPoint.Zero as topLeft)
    /// </summary>
    public static void Draw<TD>(TD drawer, BorderValue border, TvPoint topLeft, TvBounds bounds, TvColorsPair colors) where TD : IConsoleDrawer
    {
        var columns = bounds.Width;
        var rows = bounds.Height;

        var minrows = border.HasHorizontalBorder ? 2 : 0;
        var mincols = border.HasVerticalBorder ? 2 : 0;
        var borderSet = BorderSets.GetBorderSet(border);

        if (rows > minrows && columns > mincols)
        {
            var displacement = TvPoint.FromXY(border.HasVerticalBorder ? 1 : 0, border.HasHorizontalBorder ? 1 : 0);
            var adjustement = TvBounds.FromRowsAndCols(border.HasHorizontalBorder ? 2 : 0, border.HasVerticalBorder ? 2 : 0);


            if (border.HasHorizontalBorder)
            {
                drawer.DrawChars(borderSet[BorderSets.Entries.TOPLEFT], 1, topLeft + TvPoint.Zero, colors);
                drawer.DrawChars(borderSet[BorderSets.Entries.TOPRIGHT], 1, topLeft + TvPoint.FromXY(columns - 1, 0), colors);
                drawer.DrawChars(borderSet[BorderSets.Entries.HORIZONTAL], columns - 2, topLeft + TvPoint.FromXY(1, 0), colors);
                drawer.DrawChars(borderSet[BorderSets.Entries.BOTTOMLEFT], 1, topLeft + TvPoint.FromXY(0, rows - 1), colors);
                drawer.DrawChars(borderSet[BorderSets.Entries.BOTTOMRIGHT], 1, topLeft + TvPoint.FromXY(columns - 1, rows - 1), colors);
                drawer.DrawChars(borderSet[BorderSets.Entries.HORIZONTAL], columns - 2, topLeft + TvPoint.FromXY(1, rows - 1), colors);
            }
            if (border.HasVerticalBorder)
            {
                var startRow = border.HasHorizontalBorder ? 1 : 0;
                var endrow = border.HasHorizontalBorder ? rows - 1 : rows - 2;
                for (var row = startRow; row < endrow; row++)
                {
                    drawer.DrawChars(borderSet[BorderSets.Entries.VERTICAL], 1, topLeft + TvPoint.FromXY(0, row), colors);
                    drawer.DrawChars(borderSet[BorderSets.Entries.VERTICAL], 1, topLeft + TvPoint.FromXY(columns - 1, row), colors);
                }
            }
        }
    }
}
