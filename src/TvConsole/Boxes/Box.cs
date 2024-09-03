using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;

namespace Tvision2.Console.Boxes;

public readonly record struct Box(TvPoint TopLeft, TvBounds Bounds) : IShape
{
    public TvPoint BottomRight => TopLeft + Bounds;
    public Box (int left, int top, int rows, int columns) : this(TvPoint.FromXY(left, top), TvBounds.FromRowsAndCols(rows, columns)) { }

    public Box Displace(int rows, int columns) => this with { TopLeft = TopLeft + TvPoint.FromXY(columns, rows) };
    public Box MoveTo(int x, int y) => this with { TopLeft = TvPoint.FromXY(x, y) };
    public Box MoveTo(TvPoint point) => this with { TopLeft = point };

    public Box Stretch(int rows, int columns) => this with { Bounds = Bounds + TvBounds.FromRowsAndCols(rows, columns) };
    public Box Shrink(int rows, int columns) => this with { Bounds = Bounds - TvBounds.FromRowsAndCols(rows, columns) };
    public Box Resize(int rows, int columns) => this with { Bounds = TvBounds.FromRowsAndCols(rows, columns) };
    public Box Resize(TvBounds bounds) => this with { Bounds = bounds };

    public bool PointIsInside(TvPoint point) =>
        point.X > TopLeft.X && point.X < BottomRight.X && point.Y > TopLeft.Y && point.Y < BottomRight.Y;
}

public interface IShape
{
    bool PointIsInside(TvPoint point);
    TvPoint TopLeft { get; }
    TvBounds Bounds { get; }
    TvPoint BottomRight => TopLeft + Bounds;
}