using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core;
using Tvision2.Engine.Components.Backgrounds;

namespace Tvision2.Engine.Extensions;

public static class Tvision2OptionsExtensions
{
    public static ITvision2Options WithBackground(this ITvision2Options options, TvColor bgColor)
    {
        var backgroundDefinition = DefaultBackgroundDefinitionsProvider.SolidColorBackground(bgColor);
        return options.WithBackground(backgroundDefinition);
    }
}