using System.Globalization;
using System.Net.Mime;
using Tvision2.Core;
using Tvision2.Drawing;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Shapes;

namespace Tvision2.Console;

partial class TvConsole
{
    public static void DrawBox(int left, int top, int rows, int columns, BorderValue? value  = null)
    {
        var topLeft = TvPoint.FromXY(left, top);
        var bounds = TvBounds.FromRowsAndCols(rows, columns);
        var colors = TvColorsPair.FromForegroundAndBackground(Foreground, Background);
        BorderDrawer.Draw(ConsoleDrawer, value ?? BorderValue.Double(),  topLeft, bounds, colors);
    }

    public static void Draw(Box box, IDynamicColor? fgColor = null, IDynamicColor? bgColor = null)
    {
        var fg = fgColor ?? SolidDynamicColor.FromColor(Foreground);
        var bg = bgColor ?? SolidDynamicColor.FromColor(Background);
        BorderDrawer.Draw(ConsoleDrawer, box.Border, box.TopLeft, box.Bounds, fg, bg);
    }

    public static void Write<TShape>(string msg, TShape shape, IPositionResolver positionResolver) where TShape : IShape
    {
        var len = new StringInfo(msg).LengthInTextElements;
        var pos = positionResolver.Resolve(shape.BoundsInside, TvBounds.FromRowsAndCols(1, len));
        Write(msg, shape.TopLeftInside.Y +  pos.Y, shape.TopLeftInside.X +  pos.X);
    }
}