using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcwidth;

namespace Tvision2.Drawing;

static class Length
{
    public static int WidthFromString(string str)
    {
        var runes = str.EnumerateRunes();
        var len = 0;
        foreach (var rune in runes)
        {
            len += UnicodeCalculator.GetWidth(rune);
        }

        return len;
    }

    public static int WidthFromRunes(ReadOnlySpan<Rune> runes)
    {
        var len = 0;
        for (var idx = 0; idx < runes.Length; idx++)
        {
            len += UnicodeCalculator.GetWidth(runes[idx]);
        }
        return len;
    }

    public static int WidthFromRunes(IEnumerable<Rune> runes)
    {
        var len = 0;
        foreach (var rune in runes)
        {
            len += UnicodeCalculator.GetWidth(rune);
        }

        return len;
    }
}
