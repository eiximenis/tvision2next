using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using static System.Net.Mime.MediaTypeNames;

namespace Tvision2.Engine.Render;



public interface IPositionResolver
{
    TvPoint Resolve(TvBounds containerBounds, TvBounds containedBounds);
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
    public static HCenteredTextResolver CenterHorizontally(int row = 0, int margin = 0) => new HCenteredTextResolver(row, Margin.FromValue(margin));
    public static HCenteredTextResolver CenterHorizontally(int row = 0, Margin margin = new ()) => new HCenteredTextResolver(row, margin);
}

public readonly record struct HCenteredTextResolver(int Row, Margin Margin) : IPositionResolver
{
    public TvPoint Resolve(TvBounds containerBounds, TvBounds innerBounds)
    {
        var leftMargin = Margin.Left;
        var rightMargin = Margin.Right;
        var columns = containerBounds.Width - leftMargin - rightMargin;
        var length = innerBounds.Width;

        if (length >= columns) return TvPoint.FromXY(leftMargin, Row);
        var start = ((columns - length) / 2) + leftMargin;
        return TvPoint.FromXY(start, Row);

    }
}
