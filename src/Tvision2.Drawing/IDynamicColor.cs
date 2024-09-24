using Tvision2.Core;

namespace Tvision2.Drawing;

public interface IDynamicColor
{
    bool IsFixedColor  => false;
    TvColor GetColorForPosition(TvPoint point);
}