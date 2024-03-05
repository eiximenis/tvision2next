using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Engine;

namespace Tvision2.Styles.Extensions;

public static class IHostBuilderExtensions_Styles
{

    public static IHostBuilder AddStyles(this IHostBuilder builder, Action<ITvStylesBuilder>? builderConf = null)
    {
        var stylesBuilder = new StylesBuilder();
        builderConf?.Invoke(stylesBuilder);
        var styles = stylesBuilder.Build();
        builder.ConfigureServices(s =>
        {
            s.AddSingleton(sp => new StylesManager(styles, sp.GetRequiredService<Tvision2Engine>()));
            s.ActivateSingleton<StylesManager>();
        });
       
        return builder;
    }
    
}