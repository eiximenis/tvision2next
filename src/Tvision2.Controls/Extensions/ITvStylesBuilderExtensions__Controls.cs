using Tvision2.Controls.Extensions.Styles;
using Tvision2.Styles;

namespace Tvision2.Controls.Extensions;

public static class ITvStylesBuilderExtensions__Controls
{
    public static TvControlStylesBuilder WithControlStyles(this ITvStylesBuilder builder)
    {
        var controlsSet = builder.WithStyleSet("TvControls");
        return new TvControlStylesBuilder(controlsSet);
    }

}
