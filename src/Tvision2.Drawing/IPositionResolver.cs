using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;

namespace Tvision2.Drawing;

public interface IPositionResolver
{
    TvPoint Resolve(TvBounds containerBounds, TvBounds containedBounds, TvPoint containerTopLeft);
}
public readonly record struct Margin(int Left = 0, int Right = 0, int Top = 0, int Bottom = 0)
{
    public static Margin FromValue(int value) => new Margin(value, value, value, value);
    public static Margin LeftMargin(int value) => new Margin(Left: value);

    public static Margin RightMargin(int value) => new Margin(Right: value);
    public static Margin TopMargin(int value) => new Margin(Top: value);
    public static Margin BottomMargin(int value) => new Margin(Bottom: value);
}
public static class TextPosition
{
    private static readonly BottomTextPosition _bottom = new BottomTextPosition();
    private static readonly TopTextPosition _top = new TopTextPosition();   

    public static TopTextPosition Top() => _top;
    public static BottomTextPosition Bottom() => _bottom;
}

public class TopTextPosition
{
    public TopCenteredTextResolver CenterHorizontally(int margin = 0, int row = 0) => new TopCenteredTextResolver(row, Margin.FromValue(margin));
    public TopCenteredTextResolver CenterHorizontally(Margin margin, int row = 0) => new TopCenteredTextResolver(row, margin);
}

public class BottomTextPosition
{
    public BottomCenteredTextResolver CenterHorizontally(int margin = 0) => new BottomCenteredTextResolver();
}

public readonly record struct TopCenteredTextResolver(int Row, Margin Margin) : IPositionResolver
{
    public TvPoint Resolve(TvBounds containerBounds, TvBounds innerBounds, TvPoint containerTopLeft)
    {
        var leftMargin = Margin.Left;
        var rightMargin = Margin.Right;
        var columns = containerBounds.Width - leftMargin - rightMargin;
        var length = innerBounds.Width;

        if (length >= columns) return TvPoint.FromXY(leftMargin, Row);
        var start = (columns - length) / 2 + leftMargin;
        return TvPoint.FromXY(start, Row) + containerTopLeft;

    }
}

public readonly record struct BottomCenteredTextResolver() : IPositionResolver
{
    public TvPoint Resolve(TvBounds containerBounds, TvBounds innerBounds, TvPoint containerTopLeft)
    {
        var columns = containerBounds.Width;
        var length = innerBounds.Width;

        if (length >= columns) return TvPoint.FromXY(0, containerBounds.Height - 1);
        var start = (columns - length) / 2;
        return TvPoint.FromXY(start, containerBounds.Height - 2) + containerTopLeft;
    }
}