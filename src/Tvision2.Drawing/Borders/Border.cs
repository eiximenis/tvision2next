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
    public static void Draw<TD>(TD drawer, BorderValue border, TvPoint topLeft, TvBounds bounds, TvColorsPair colors) where TD : IConsoleDrawer
    {
        var fgColor = SolidDynamicColor.FromColor(colors.Foreground);
        var bgColor = SolidDynamicColor.FromColor(colors.Background);
        Draw(drawer, border, topLeft, bounds, fgColor, bgColor);
    }
    /// <summary>
    /// Draws a border in the specified location with the specified bounds and colors
    /// topLeft parameter is console coordinates (if [TD]==ConsoleContextDrawer then usually should be TvPoint.Zero).
    /// </summary>
    public static void Draw<TD>(TD drawer, BorderValue border, TvPoint topLeft, TvBounds bounds, IDynamicColor fgColor, IDynamicColor bgColor) where TD : IConsoleDrawer
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

                drawer.DrawChars(borderSet[BorderSets.Entries.TOPLEFT], 1, topLeft + TvPoint.Zero, fgColor, bgColor);
                drawer.DrawChars(borderSet[BorderSets.Entries.TOPRIGHT], 1, topLeft + TvPoint.FromXY(columns - 1, 0), fgColor, bgColor);
                drawer.DrawChars(borderSet[BorderSets.Entries.HORIZONTAL], columns - 2, topLeft + TvPoint.FromXY(1, 0), fgColor, bgColor);
                drawer.DrawChars(borderSet[BorderSets.Entries.BOTTOMLEFT], 1, topLeft + TvPoint.FromXY(0, rows - 1), fgColor, bgColor);
                drawer.DrawChars(borderSet[BorderSets.Entries.BOTTOMRIGHT], 1, topLeft + TvPoint.FromXY(columns - 1, rows - 1), fgColor, bgColor);
                drawer.DrawChars(borderSet[BorderSets.Entries.HORIZONTAL], columns - 2, topLeft + TvPoint.FromXY(1, rows - 1), fgColor, bgColor);
            }
            if (border.HasVerticalBorder)
            {
                var startRow = border.HasHorizontalBorder ? 1 : 0;
                var endrow = border.HasHorizontalBorder ? rows - 1 : rows - 2;
                for (var row = startRow; row < endrow; row++)
                {
                    drawer.DrawChars(borderSet[BorderSets.Entries.VERTICAL], 1, topLeft + TvPoint.FromXY(0, row), fgColor, bgColor);
                    drawer.DrawChars(borderSet[BorderSets.Entries.VERTICAL], 1, topLeft + TvPoint.FromXY(columns - 1, row), fgColor, bgColor);
                }
            }
        }
    }
}
