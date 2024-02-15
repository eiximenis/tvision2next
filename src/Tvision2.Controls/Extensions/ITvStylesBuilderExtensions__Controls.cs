using Tvision2.Styles;
using Tvision2.Styles.Builder;

namespace Tvision2.Controls.Extensions;

public static class ITvStylesBuilderExtensions__Controls
{
    public static TvControlStylesBuilder WithControlStyles(this ITvStylesBuilder builder)
    {
        var controlsSet = builder.WithStyleSet("TvControls");
        return new TvControlStylesBuilder(controlsSet);
    }

}

public class TvControlStylesBuilder
{
    private readonly StyleSetDefinition _controlsSetDefinition;
    public TvControlStylesBuilder(StyleSetDefinition controlsSetDefinition)
    {
        _controlsSetDefinition = controlsSetDefinition;
    }
    
    
}