using Tvision2.Core;

namespace Tvision2.Styles;

public sealed class StyleState
{
    private readonly TvColor? _fixedBg;
    private readonly TvColor? _fixedFg;
    private readonly IDynamicColor _foreground;
    private readonly IDynamicColor _background;
    

    public string Name { get; }

    public bool ForegroundFixed => _foreground.IsFixedColor;
    public bool BackgroundFixed => _background.IsFixedColor;

    public StyleState(string name, IDynamicColor fore, IDynamicColor back)
    {
        Name = name;
        _foreground = fore;
        _background = back;
        if (_foreground.IsFixedColor)
        {
            _fixedFg = _foreground.GetColorForPosition(TvPoint.Zero);
        }

        if (_background.IsFixedColor)
        {
            _fixedBg = _background.GetColorForPosition(TvPoint.Zero);
        }
    }

    public TvColorsPair ColorsPairAt(TvPoint position)
    {
        return TvColorsPair.FromForegroundAndBackground(
            _fixedFg ?? _foreground.GetColorForPosition(position),
            _fixedBg ?? _background.GetColorForPosition(position));
    }

    public TvColor ForegroundAt(TvPoint position) => _fixedFg ?? _foreground.GetColorForPosition(position);
    public TvColor BackgroundAt(TvPoint position) => _fixedBg ?? _background.GetColorForPosition(position);
}