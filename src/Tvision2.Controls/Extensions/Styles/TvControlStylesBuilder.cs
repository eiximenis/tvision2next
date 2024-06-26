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

        if (!csdef.ContainsState("Normal"))
        {
            csdef.WithControlState(ControlStyleState.Normal).UseColors(TvColor.Yellow, TvColor.Blue);
        }

        if (!csdef.ContainsState("Focused"))
        {
            csdef.WithControlState(ControlStyleState.Focused).UseColors(TvColor.LightWhite, TvColor.Blue);
        }
    }

    public StyleDefinition Default() => _controlsSetDefinition.WithDefaultStyle();

    public StyleDefinition AddStyleFor<T>() where T : ITvControl
    {
        var typeName = typeof(T).Name;
        return _controlsSetDefinition.WithStyle(typeName);
    }

}