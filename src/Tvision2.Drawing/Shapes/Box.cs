using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;

namespace Tvision2.Drawing.Shapes;

public readonly record struct Box(TvPoint TopLeft, TvBounds Bounds, BorderValue Border) : IShape
{
    public TvPoint BottomRight => TopLeft + Bounds;
    public Box(int left, int top, int rows, int columns) : this(TvPoint.FromXY(left, top), TvBounds.FromRowsAndCols(rows, columns), BorderValue.Double()) { }
    public Box(int left, int top, int rows, int columns, BorderValue border) : this(TvPoint.FromXY(left, top), TvBounds.FromRowsAndCols(rows, columns), border) { }
    public Box(TvPoint topLeft, TvBounds bounds) : this(topLeft, bounds, BorderValue.Double()) { }

    public Box Displace(int rows, int columns) => this with { TopLeft = TopLeft + TvPoint.FromXY(columns, rows) };
    public Box MoveTo(int x, int y) => this with { TopLeft = TvPoint.FromXY(x, y) };
    public Box MoveTo(TvPoint point) => this with { TopLeft = point };

    public Box Stretch(int rows, int columns) => this with { Bounds = Bounds + TvBounds.FromRowsAndCols(rows, columns) };
    public Box Shrink(int rows, int columns) => this with { Bounds = Bounds - TvBounds.FromRowsAndCols(rows, columns) };
    public Box Resize(int rows, int columns) => this with { Bounds = TvBounds.FromRowsAndCols(rows, columns) };
    public Box Resize(TvBounds bounds) => this with { Bounds = bounds };

    public TvPoint TopLeftInside => TopLeft + TvPoint.FromXY(Border.HasVerticalBorder ? 1 : 0, Border.HasVerticalBorder ? 1 : 0);
    public TvPoint BottomRightInside => BottomRight - TvPoint.FromXY(Border.HasVerticalBorder ? 1 : 0, Border.HasVerticalBorder ? 1 : 0);

    public bool PointIsInside(TvPoint point) =>
        point.X > TopLeft.X && point.X < BottomRight.X && point.Y > TopLeft.Y && point.Y < BottomRight.Y;
}

public static class ShapeExtensions
{
    public static int GetLineLength<TShape>(this TShape shape, int row) where TShape : IShape
    {
        var len = 0;
        for (var col = shape.TopLeft.X; col <= shape.BottomRight.X; col++)
        {
            if (shape.PointIsInside(TvPoint.FromXY(col, row)))
            {
                len++;
            }
        }

        return len;
    }

    public static IEnumerable<int> GetLinesLengths<TShape>(this TShape shape) where TShape : IShape
    {
        for (var line = shape.TopLeftInside.Y; line <= shape.BottomRightInside.Y; line++)
        {
            yield return shape.GetLineLength(line);
        }

    }
}
