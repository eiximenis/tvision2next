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

public static class TextPosition
{
    public static HCenteredTextResolver CenterHorizontally(int row = 0, int margin = 0) => new HCenteredTextResolver(row, margin);
}

public readonly record struct HCenteredTextResolver(int Row, int Margin) : IPositionResolver
{
    public TvPoint Resolve(TvBounds containerBounds, TvBounds innerBounds)
    {
        var columns = containerBounds.Width - Margin * 2;
        var length = innerBounds.Width;

        if (length >= columns) return TvPoint.FromXY(Margin, Row);
        var start = (columns - length / 2) + Margin;
        return TvPoint.FromXY(start, Row);

    }
}
