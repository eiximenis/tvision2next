using System.Globalization;
using System.Net.Mime;
using Tvision2.Console.Boxes;
using Tvision2.Core;
using Tvision2.Drawing;
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

    public static void Draw(Box box)
    {
        var colors = TvColorsPair.FromForegroundAndBackground(Foreground, Background);
        Border.Draw(ConsoleDrawer, BorderValue.Double(), box.TopLeft, box.Bounds, colors);
    }

    public static void Write<TShape>(string msg, TShape shape, IPositionResolver positionResolver) where TShape : IShape
    {
        var len = new StringInfo(msg).LengthInTextElements;
        var pos = positionResolver.Resolve(shape.Bounds, TvBounds.FromRowsAndCols(1, len), shape.TopLeft);
        Write(msg, pos.Y, pos.X);
    }
}