using Tvision2.Core;
using Tvision2.Styles.Builder;

namespace Tvision2.Controls.Extensions.Styles;

public class TvControlStylesBuilder
{
    private readonly StyleSetDefinition _controlsSetDefinition;
    public TvControlStylesBuilder(StyleSetDefinition controlsSetDefinition)
    {
        _controlsSetDefinition = controlsSetDefinition;
        var csdef = _controlsSetDefinition.WithDefaultStyle();

        csdef.WithControlState(ControlStyleState.Normal).UseColors(TvColor.Yellow, TvColor.Blue);
        csdef.WithControlState(ControlStyleState.Focused).UseColors(TvColor.LightWhite, TvColor.Blue);
    }

    public StyleDefinition AddStyleFor<T>() where T : ITvControl
    {
        var typeName = typeof(T).Name;
        return _controlsSetDefinition.WithStyle(typeName);
    }

}