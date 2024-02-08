using Tvision2.Core;
using Tvision2.Engine.Render;

namespace Tvision2.Styles;

public  readonly ref  struct StyledConsoleContext
{
    private readonly ConsoleContext _consoleContext;
    private  readonly  Style _style;

    internal StyledConsoleContext(ConsoleContext consoleContext, Style style)
    {
        _consoleContext = consoleContext;
        _style = style;
    }
    
    public void DrawStringAt(string text, TvPoint location) =>
        _consoleContext.DrawStringAt(text, location, _style.DefaultState.ColorsPairAt(location));

    public void Fill()
    {
        var state = _style.DefaultState;
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

    public void DrawString(string text, TvPoint location, string stateName) =>
        _consoleContext.DrawStringAt(text, location, _style[stateName].ColorsPairAt(location));

}