using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tvision2.Styles.Extensions;

public static class IHostBuilderExtensions_Styles
{

    public static IHostBuilder AddTvControls(this IHostBuilder builder, Action<ITvStylesBuilder>? builderConf = null)
    {
        var stylesBuilder = new StylesBuilder();
        builderConf?.Invoke(stylesBuilder);
        var styles = stylesBuilder.Build();
        var manager = new StylesManager(styles);
        builder.ConfigureServices(s => s.AddSingleton(manager));
        return builder;
    }
    
}