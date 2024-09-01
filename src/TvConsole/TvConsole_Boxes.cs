using Tvision2.Core;
using Tvision2.Drawing.Borders;

namespace Tvision2.Console;

partial class TvConsole
{
    public static void DrawBox(int left, int top, int rows, int columns)
    {
        var topLeft = TvPoint.FromXY(left, top);
        var bounds = TvBounds.FromRowsAndCols(rows, columns);
        var colors = TvColorsPair.FromForegroundAndBackground(Foreground, Background);
        Border.Draw(ConsoleDrawer, BorderValue.Double(), topLeft, bounds, colors);
    }
}