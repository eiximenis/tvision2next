using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Console;
using Tvision2.Drawing;
using Tvision2.Engine.Components;
using Tvision2.Engine.Drawing;
using Tvision2.Engine.Render;

namespace Tvision2.Styles;

public readonly struct StyledConsoleContext
{
    private readonly ConsoleContext _consoleContext;
    private readonly StyleSet _styleSet;

    public Viewzone Viewzone => _consoleContext.Viewzone;

    internal StyledConsoleContext(ConsoleContext consoleContext, StyleSet styleSet)
    {
        _consoleContext = consoleContext;
        _styleSet = styleSet;
    }

    public void DrawStringAt(string text, TvPoint location) => DrawStringAt(text, location, _styleSet.DefaultStyle.DefaultState);
    public void DrawStringAt(string text, TvPoint location, string stateName) => DrawStringAt(text, location, _styleSet.DefaultStyle.GetStateOrDefault(stateName));

    public void DrawStringAt(string text, TvPoint location, string styleName, string stateName) => DrawStringAt(text, location, _styleSet[styleName].GetStateOrDefault(stateName));

    private void DrawStringAt(string text, TvPoint location, StyleState state) => _consoleContext.DrawStringAt(text, location, state.ColorsPairAt(location));

    public void DrawCharsAt(char value, int count, TvPoint location) => _consoleContext.DrawCharsAt(value, count, location, _styleSet.DefaultStyle.DefaultState.ColorsPairAt(location));
    public void DrawCharsAt(char value, int count, TvPoint location, StyleState state) => _consoleContext.DrawCharsAt(value, count, location, state.ColorsPairAt(location));

    public void Fill() => Fill(_styleSet.DefaultStyle.DefaultState);
    public void Fill(string stateName) => Fill(_styleSet.DefaultStyle.GetStateOrDefault(stateName));
    public void Fill(string styleName, string stateName) => Fill(_styleSet[styleName].GetStateOrDefault(stateName));

    // Duplicate ConsoleContext functions to support "override styling" with fixed colors
    public void Fill(TvColor color) => _consoleContext.Fill(color);
    public void DrawCharsAt(char value, int count, TvPoint location, TvColorsPair colors) => _consoleContext.DrawCharsAt(value, count, location, colors);
    public void DrawRunesAt(Rune rune, int count, TvPoint location, CharacterAttribute attribute) => _consoleContext.DrawRunesAt(rune, count, location, attribute);
    public void DrawStringAt(string text, TvPoint location, TvColorsPair colors) => _consoleContext.DrawStringAt(text, location, colors);


    public ConsoleContextDrawer GetConsoleDrawer() => _consoleContext.GetConsoleDrawer();

    private void Fill(StyleState state)
    {

        if (state.BackgroundFixed)
        {
            _consoleContext.Fill(state.BackgroundAt(TvPoint.Zero));
        }
        else
        {
            for (var row = 0; row < _consoleContext.Viewzone.Bounds.Height; row++)
            {
                for (var col = 0; col < _consoleContext.Viewzone.Bounds.Width; col++)
                {
                    var pos = TvPoint.FromXY(col, row);
                    _consoleContext.DrawStringAt(" ", pos, state.ColorsPairAt(pos));
                }
            }
        }
    }

}

public static class StyledContextExtensions_Drawing
{
    public static TvPoint GetPositionForString<TPR>(this StyledConsoleContext ctx, string text, TPR locationResolver) where TPR : IPositionResolver
    {
        var info = new StringInfo(text);
        var length = info.LengthInTextElements;
        return locationResolver.Resolve(ctx.Viewzone.Bounds, TvBounds.FromRowsAndCols(1, length));
    }
}