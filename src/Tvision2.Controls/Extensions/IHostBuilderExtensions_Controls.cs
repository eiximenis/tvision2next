using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Engine.Extensions;

namespace Tvision2.Controls.Extensions;

public static class IHostBuilderExtensions_Controls
{
    public static IHostBuilder AddTvControls(this IHostBuilder builder, Action<ITvControlsOptions>? controlsOptions = null)
    {
        var options = new TvControlsOptions();
        controlsOptions?.Invoke(options);
        builder.ConfigureServices(s =>
        {
            s.AddSingleton<TvControlsOptions>(options);
            s.AddSingleton<TvControlsTree>();
            s.AddTvisionHook<TvControlsEventsHook>();
        });
        return builder;
    } 
}