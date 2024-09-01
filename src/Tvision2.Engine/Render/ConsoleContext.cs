using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Console;
using Tvision2.Engine.Components;
using Wcwidth;

namespace Tvision2.Engine.Render;

public readonly struct ConsoleContext
{
    private readonly VirtualConsole _console;
    private readonly Viewport _viewport;
    public ConsoleContext(VirtualConsole console, Viewport viewport)
    {
        _console = console;
        _viewport = viewport;
    }

    public Viewzone Viewzone => _viewport.Viewzone;
    
    
    private static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition) => viewPoint + viewportPosition;
    private static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition) => consolePoint - viewportPosition;

    public void DrawCharsAt(char value, int count, TvPoint location, CharacterAttribute attribute)
    {
        var consoleLocation = ViewPointToConsolePoint(location, _viewport.Position);
        _console.DrawCharactersAt(value,count, consoleLocation, attribute, _viewport.Viewzone);
    }
    public void DrawRunesAt(Rune rune, int count, TvPoint location, CharacterAttribute attribute)
    {
        var consoleLocation = ViewPointToConsolePoint(location, _viewport.Position);
        _console.DrawRunesAt(rune, count, consoleLocation, attribute, _viewport.Viewzone);
    }

    public void DrawStringAt(string text, TvPoint location, TvColorsPair colors)
    {
        var attr = new CharacterAttribute(colors.Foreground, colors.Background, CharacterAttributeModifiers.Normal);
        var consoleLocation = ViewPointToConsolePoint(location, _viewport.Position);
        _console.DrawAt(text,consoleLocation, attr, _viewport.Viewzone);
    }

    public TvPoint GetPositionForString<TPR>(string text, TPR locationResolver) where TPR : IPositionResolver
    {
        var info = new StringInfo(text);
        var length = info.LengthInTextElements;
        return locationResolver.Resolve(_viewport.Bounds, TvBounds.FromRowsAndCols(1, length));
    }

    public void Fill(TvColor bgColor)
    {
        var attr = new CharacterAttribute(bgColor, bgColor, CharacterAttributeModifiers.Normal);
        var consoleLocation = ViewPointToConsolePoint(TvPoint.Zero, _viewport.Position);
        _console.FillRect(consoleLocation, attr, _viewport.Viewzone);
    }

    internal ConsoleContext WithDrawResultsApplied(DrawResult drawResult)
    {
        var newViewport = new Viewport(_viewport.Position + drawResult.Displacement, _viewport.Bounds.Reduced(drawResult.BoundsAdjustment));
        return new ConsoleContext(_console, newViewport);
    }
}