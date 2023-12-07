namespace Tvision2.Core;

public readonly record struct TvPoint(int X, int Y)
{
    public static TvPoint Zero => new TvPoint(0, 0);
    public override string ToString() => $"({X},{Y})";

    public static TvPoint FromXY(int x, int y) => new TvPoint(x, y);

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public static TvPoint Row(int row) => new TvPoint(0, row);
    public static TvPoint Column(int col) => new TvPoint(col, 0);

    public static TvPoint operator +(TvPoint first, TvPoint second)
    {
        return new TvPoint(first.X + second.X, first.Y + second.Y);
    }

    public static TvPoint operator -(TvPoint first, TvPoint second)
    {
        return new TvPoint(first.X - second.X, first.Y - second.Y);
    }

    public static TvBounds CalculateBounds(TvPoint topLeft, TvPoint bottomRight)
    {

        var rows = bottomRight.Y >= topLeft.Y ? bottomRight.Y - topLeft.Y  + 1: 0;
        var cols = bottomRight.X >= topLeft.X ? bottomRight.X - topLeft.X  + 1: 0;
        return TvBounds.FromRowsAndCols(rows, cols);
    }

    public static TvPoint operator +(TvPoint point, TvBounds bounds)
    {
        return bounds switch
        {
            (0, 0) => point,
            (var w, 0) => TvPoint.FromXY(point.X + w - 1, point.Y),
            (0, var h)  => TvPoint.FromXY(point.X, point.Y + h - 1),
            _ => TvPoint.FromXY(point.X + bounds.Width - 1, point.Y + bounds.Height - 1)
        };
    }
}