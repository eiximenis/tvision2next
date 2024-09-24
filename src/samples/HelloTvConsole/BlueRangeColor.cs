using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing;

namespace HelloTvConsole;
class BlueRangeColor : IDynamicColor
{
    private readonly TvPoint _initial;
    public BlueRangeColor(TvPoint initial)
    {
        _initial = initial;
    }

    public TvColor GetColorForPosition(TvPoint point)
    {
        var blue = (byte)((point.X - _initial.X + point.Y - _initial.Y) *  6);
        return TvColor.FromRgb((byte)0, (byte)blue, blue);
    }
}
