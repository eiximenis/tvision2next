using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Core.Console;
using Tvision2.Drawing;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Drawing;


/// <summary>
/// This class is for allowing interaction with Tvision2.Drawing package
/// This allows to share code between TvConsole and Tvision2 drawers
///
/// Important: When creating Tvision2 drawers ensure all calls to Tvision2.Drawing
/// use TvPoint.Zero as location parameter. This is because the location is already
/// based on the position of the ConsoleContext itself.
/// </summary>
public class ConsoleContextDrawer : IConsoleDrawer
{
    private readonly ConsoleContext _ctx;

    public ConsoleContextDrawer(in ConsoleContext ctx)
    {
        _ctx = ctx;
    }
    public void DrawStringAt(string text, TvPoint location, TvColorsPair colors)
    {
        _ctx.DrawStringAt(text, location, colors);
    }

    public void DrawChars(char character, int count, TvPoint location, TvColorsPair colors)
    {
        _ctx.DrawCharsAt(character, count, location, colors);
    }
}
