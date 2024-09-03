using System.Drawing;
using Tvision2.Console.Boxes;
using Tvision2.Core;

namespace Tvision2.Console;

partial class TvConsole
{
    public static void Fill<TShape>(TShape shapeToFill, string? color = null) where TShape : IShape
    {
        var fillColor = color is null ? Background : TvColor.FromHexString(color);
        Fill(shapeToFill, fillColor);
    }

    public static void Fill<TShape>(TShape shapeToFill, TvColor fillColor) where TShape : IShape
    {
        var oldBg = Background;
        Background = fillColor;
        for (var row = shapeToFill.TopLeft.Y; row < shapeToFill.BottomRight.Y; row++)
        {
            for (var col = shapeToFill.TopLeft.X; col < shapeToFill.BottomRight.X; col++)
            {
                if (shapeToFill.PointIsInside(TvPoint.FromXY(col, row)))
                {
                    MoveCursorTo(col, row);
                    Write(" ");
                }
            }
        }
        Background = oldBg;
    }
}