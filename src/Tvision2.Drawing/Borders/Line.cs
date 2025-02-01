using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;

namespace Tvision2.Drawing.Borders;

public enum LineType
{
    Single,
    Double,
    Thick,
    Dashed,
    Dotted,
    Asterisk
}

public readonly record struct HorizontalLine(LineType LineType, BorderType Lateral)
{
    public void Draw<TD>(TD drawer, TvPoint pos, int width, TvColorsPair colors) where TD : IConsoleDrawer
    {
        var character = LineType switch
        {
            LineType.Single => '─',
            LineType.Double => '═',
            LineType.Thick => '━',
            LineType.Dashed => '┄',
            LineType.Dotted => '┅',
            LineType.Asterisk => '*',
            _ => throw new InvalidOperationException("Invalid line type")
        };

        drawer.DrawChars(character, width, pos, colors);
    }
}

public readonly record struct VerticalLine(LineType LineType, BorderType Lateral)
{
    public void Draw<TD>(TD drawer, TvPoint pos, int height, TvColorsPair colors) where TD : IConsoleDrawer
    {
        var character = LineType switch
        {
            LineType.Single => '│',
            LineType.Double => '║',
            LineType.Thick => '┃',
            LineType.Dashed => '┆',
            LineType.Dotted => '┇',
            LineType.Asterisk => '*',
            _ => throw new InvalidOperationException("Invalid line type")
        };

        for (var idx = 0; idx < height; idx++)
        {
            drawer.DrawChars(character, 1, pos + TvPoint.FromXY(0, idx), colors);
        }
    }
}