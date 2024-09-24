using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing;

namespace Tvision2.Console;
class TvConsoleDrawer : IConsoleDrawer
{
    public void DrawStringAt(string text, TvPoint location, TvColorsPair colors)
    {
        TvConsole.Foreground = colors.Foreground;
        TvConsole.Background = colors.Background;
        TvConsole.MoveCursorTo(location.Y, location.X);
        TvConsole.Write(text);
    }
    public void DrawStringAt(string text, TvPoint location, IDynamicColor fgColor, IDynamicColor bgColor)
    {
        var runes = text.EnumerateRunes();
        foreach (var rune in runes)
        {
            var fg = fgColor.GetColorForPosition(location);
            var bg = bgColor.GetColorForPosition(location);
            TvConsole.Write(rune, location.Y, location.X, fg, bg);
            location = location with { X = location.X + 1 };
        }
    }

    public void DrawChars(char character, int count, TvPoint location, IDynamicColor fgColor, IDynamicColor bgColor)
    {

        for (var idx = 0; idx < count; idx++)
        {
            var fg = fgColor.GetColorForPosition(location);
            var bg = bgColor.GetColorForPosition(location);
            TvConsole.Write(character, location.Y, location.X, fg, bg);
            location = location with { X = location.X + 1 };
        }
    }

    public void DrawRunes(Rune rune, int count, TvPoint location, IDynamicColor fgColor, IDynamicColor bgColor)
    {

        for (var idx = 0; idx < count; idx++)
        {
            var fg = fgColor.GetColorForPosition(location);
            var bg = bgColor.GetColorForPosition(location);
            TvConsole.Write(rune, location.Y, location.X, fg, bg);
            location = location with { X = location.X + 1 };
        }
    }



    public void DrawChars(char character, int count, TvPoint location, TvColorsPair colors)
    {
        TvConsole.Foreground = colors.Foreground;
        TvConsole.Background = colors.Background;
        TvConsole.MoveCursorTo(location.X, location.Y);
        if (count == 1)
        {
            TvConsole.Write(character);
        }
        else
        {
            TvConsole.Write(new string(character, count));
        }
    }

    public void DrawRunes(Rune rune, int count, TvPoint location, TvColorsPair colors)
    {
        TvConsole.Foreground = colors.Foreground;
        TvConsole.Background = colors.Background;
        TvConsole.MoveCursorTo(location.X, location.Y);
        if (count == 1)
        {
            TvConsole.Write(rune);
        }
        else
        {
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                sb.Append(rune);
            }

            TvConsole.Write(sb.ToString());
        }
    }


}
