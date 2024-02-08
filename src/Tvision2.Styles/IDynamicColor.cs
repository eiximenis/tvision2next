using Tvision2.Core;

namespace Tvision2.Styles;

public interface IDynamicColor
{
    bool IsFixedColor  => false;
    TvColor GetColorForPosition(TvPoint point);
}

sealed class SolidDynamicColor : IDynamicColor
{
    private readonly TvColor _color;
    public static IDynamicColor White { get; } = new SolidDynamicColor(TvColor.White);
    public static IDynamicColor Black { get; } = new SolidDynamicColor(TvColor.Black);
    public bool IsFixedColor => true;
    
    public SolidDynamicColor(TvColor color)
    {
        _color = color;
    }
    
    public TvColor GetColorForPosition(TvPoint point) => _color;
}   