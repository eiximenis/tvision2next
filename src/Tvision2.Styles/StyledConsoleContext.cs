using System.Runtime.InteropServices.JavaScript;
using Tvision2.Core;
using Tvision2.Engine.Render;

namespace Tvision2.Styles;

public readonly struct StyledConsoleContext
{
    private readonly ConsoleContext _consoleContext;
    private readonly StyleSet _styleSet;
    internal StyledConsoleContext(ConsoleContext consoleContext, StyleSet styleSet)
    {
        _consoleContext = consoleContext;
        _styleSet = styleSet;
    }


    public void DrawStringAt(string text, TvPoint location) => DrawStringAt(text, location, _styleSet.DefaultStyle.DefaultState);
    public void DrawStringAt(string text, TvPoint location, string stateName) => DrawStringAt(text, location, _styleSet.DefaultStyle.GetStateOrDefault(stateName));

    public void DrawStringAt(string text, TvPoint location, string styleName, string stateName) => DrawStringAt(text, location, _styleSet[styleName].GetStateOrDefault(stateName));

    private void DrawStringAt(string text, TvPoint location, StyleState state) => _consoleContext.DrawStringAt(text, location, state.ColorsPairAt(location));


    public void Fill() => Fill(_styleSet.DefaultStyle.DefaultState);
    public void Fill(string stateName) => Fill(_styleSet.DefaultStyle.GetStateOrDefault(stateName));
    public void Fill(string styleName, string stateName) => Fill(_styleSet[styleName].GetStateOrDefault(stateName));

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