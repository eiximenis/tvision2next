﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;

namespace Tvision2.Drawing;

public interface IPositionResolver
{
    TvPoint Resolve(TvBounds containerBounds, TvBounds innerBounds);
}

public static class TextPosition
{
    private static readonly BottomTextPosition _bottom = new BottomTextPosition();
    private static readonly TopTextPosition _top = new TopTextPosition();
    private static readonly MiddleTextPosition _middle = new MiddleTextPosition();

    public static TopTextPosition Top() => _top;
    public static BottomTextPosition Bottom() => _bottom;

    public static MiddleTextPosition Middle() => _middle;
}

public class TopTextPosition
{
    public TopCenteredTextResolver Center(int margin = 0) => new TopCenteredTextResolver(Margin.FromValue(margin));
    public TopCenteredTextResolver Center(Margin margin) => new TopCenteredTextResolver(margin);
}

public class BottomTextPosition
{
    public BottomCenteredTextResolver Center(int margin = 0) => new BottomCenteredTextResolver(Margin.FromValue(margin));
    public BottomCenteredTextResolver Center(Margin margin) => new BottomCenteredTextResolver(margin);
}

public class MiddleTextPosition
{
    public MiddleCenteredTextResolver Center(int margin = 0) => new MiddleCenteredTextResolver(Margin.FromValue(margin));
    public MiddleCenteredTextResolver Center(Margin margin) => new MiddleCenteredTextResolver(margin);
}


public readonly record struct MiddleCenteredTextResolver(Margin margin) : IPositionResolver
{
    public TvPoint Resolve(TvBounds containerBounds, TvBounds innerBounds)
    {
        var realHeight = innerBounds.Height - (margin.Bottom + margin.Top);
        var rowsDiff = containerBounds.Height - realHeight;

        var length = innerBounds.Width;
        var leftMargin = margin.Left;
        var rightMargin = margin.Right;
        var realColumns = containerBounds.Width - leftMargin - rightMargin;
        var startRow = rowsDiff > 0 ? rowsDiff / 2 : 0;

        if (length >= realColumns) return TvPoint.FromXY(leftMargin, startRow);
        var column = (realColumns - length) / 2 + leftMargin;
        return TvPoint.FromXY(column, startRow);

    }
}

public readonly record struct TopCenteredTextResolver(Margin margin) : IPositionResolver
{
    public TvPoint Resolve(TvBounds containerBounds, TvBounds innerBounds)
    {
        var length = innerBounds.Width;
        var startRow = margin.Top;

        var leftMargin = margin.Left;
        var rightMargin = margin.Right;
        var realColumns = containerBounds.Width - leftMargin - rightMargin;

        if (length >= realColumns) return TvPoint.FromXY(leftMargin, startRow);
        var column = (realColumns - length) / 2 + leftMargin;
        return TvPoint.FromXY(column, startRow);

    }
}

public readonly record struct BottomCenteredTextResolver(Margin margin) : IPositionResolver
{
    public TvPoint Resolve(TvBounds containerBounds, TvBounds innerBounds)
    {

        var leftMargin = margin.Left;
        var rightMargin = margin.Right;
        var realColumns = containerBounds.Width - leftMargin - rightMargin;
        var length = innerBounds.Width;
        var startRow = containerBounds.Height - 1 - margin.Bottom;

        if (length >= realColumns) return TvPoint.FromXY(leftMargin, startRow);
        var column = (realColumns - length) / 2 + leftMargin;
        return TvPoint.FromXY(column, startRow);
    }
}