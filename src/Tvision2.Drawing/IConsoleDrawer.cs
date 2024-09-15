using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;

namespace Tvision2.Drawing;
public interface IConsoleDrawer
{
    void DrawStringAt(string text, TvPoint location, TvColorsPair colors);
    void DrawChars(char character, int count, TvPoint location, TvColorsPair colors);
    void DrawRunes(Rune rune, int count, TvPoint location, TvColorsPair colors);
}
