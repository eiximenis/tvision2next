using Tvision2.Core.Console;
using Tvision2.Engine.Render;
using Wcwidth;

namespace Tvision2.Core.Engine.Render;

public readonly struct ConsoleContext
{
    private readonly VirtualConsole _console;
    private readonly Viewport _viewport;
    public ConsoleContext(VirtualConsole console, Viewport viewport)
    {
        _console = console;
        _viewport = viewport;
    }
    
    
    private static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition) => viewPoint + viewportPosition;
    private static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition) => consolePoint - viewportPosition;

    public void DrawStringAt(string text, TvPoint location, TvColorsPair colors)
    {
        var attr = new CharacterAttribute(colors.Foreground, colors.Background, CharacterAttributeModifiers.Normal);

        var consoleLocation = ViewPointToConsolePoint(location, _viewport.Position);
        var maxcols = (_viewport.Bounds.Width - location.X);

        var currentWidth = 0;
        var runes = text.EnumerateRunes();
        while (runes.MoveNext())
        {
            var rune = runes.Current;
            var runewidth = UnicodeCalculator.GetWidth(rune);
            currentWidth += runewidth < 1 ? 1 : runewidth;
        }

        maxcols = maxcols < currentWidth ? maxcols : currentWidth;
        _console.DrawAt(text, maxcols, consoleLocation, attr, _viewport.Viewzone);
    }

    public void Fill(TvColor bgColor)
    {
        var attr = new CharacterAttribute(bgColor, bgColor, CharacterAttributeModifiers.Normal);
        var consoleLocation = ViewPointToConsolePoint(TvPoint.Zero, _viewport.Position);
        _console.FillRect(consoleLocation, attr, _viewport.Viewzone);
    }
}