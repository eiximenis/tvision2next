using System.Drawing;
using Tvision2.Core;
using Tvision2.Drawing;
using Tvision2.Drawing.Shapes;

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
        for (var row = shapeToFill.TopLeftInside.Y; row <= shapeToFill.BottomRightInside.Y; row++)
        {
            for (var col = shapeToFill.TopLeft.X; col <= shapeToFill.BottomRightInside.X; col++)
            {
                if (shapeToFill.PointIsInside(TvPoint.FromXY(col, row)))
                {
                    MoveCursorTo(col, row);
                    Write(' ');
                }
            }
        }
        Background = oldBg;
    }

    public static void Fill<TShape>(TShape shapeToFill, IDynamicColor fillColor) where TShape : IShape
    {
        for (var row = shapeToFill.TopLeftInside.Y; row <= shapeToFill.BottomRightInside.Y; row++)
        {
            for (var col = shapeToFill.TopLeft.X; col <= shapeToFill.BottomRightInside.X; col++)
            {
                if (shapeToFill.PointIsInside(TvPoint.FromXY(col, row)))
                {
                    MoveCursorTo(col, row);
                    var color = fillColor.GetColorForPosition(TvPoint.FromXY(col, row));
                    Write(' ', color, color);
                }
            }
        }
    }

    public static void Clear<TShape>(TShape shapeToClear, TvColor? clearColor = null) where TShape : IShape
    {
        var oldBg = Background;
        Background = clearColor ?? Background;

        for (var row = shapeToClear.TopLeft.Y; row <= shapeToClear.BottomRight.Y; row++)
        {
            for (var col = shapeToClear.TopLeft.X; col <= shapeToClear.BottomRight.X; col++)
            {
                MoveCursorTo(col, row);
                Write(' ');
            }
        }

        Background = oldBg;
    }

    public static void Fill<TShape>(TShape shapeToFill, Func<TvPoint, TvColor> colorResolver) where TShape : IShape
    {
        var oldBg = Background;
        
        for (var row = shapeToFill.TopLeftInside.Y; row <= shapeToFill.BottomRightInside.Y; row++)
        {
            for (var col = shapeToFill.TopLeftInside.X; col <= shapeToFill.BottomRightInside.X; col++)
            {
                if (shapeToFill.PointIsInside(TvPoint.FromXY(col, row)))
                {
                    Background = colorResolver(TvPoint.FromXY(col, row) - shapeToFill.TopLeftInside);
                    MoveCursorTo(col, row);
                    Write(' ');
                }
            }
        }
        Background = oldBg;
    }
}