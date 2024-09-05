using Tvision2.Core;

namespace Tvision2.Drawing.Shapes;

public interface IShape
{
    bool PointIsInside(TvPoint point);
    TvPoint TopLeft { get; }
    TvBounds Bounds { get; }
    TvPoint BottomRight => TopLeft + Bounds;
    public int HeightInside => BottomRightInside.Y - TopLeftInside.Y + 1;
    public int WidthInside => BottomRightInside.X - TopLeftInside.X + 1;
    TvPoint TopLeftInside { get; }
    TvPoint BottomRightInside { get; }
}