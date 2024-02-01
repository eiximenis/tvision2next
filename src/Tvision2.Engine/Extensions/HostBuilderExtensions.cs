using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tvision2.Engine.Extensions;

public static class HostBuilderExtensions
{

    public static IHostBuilder UseTvision2(this IHostBuilder builder, Action<ITvision2Options>? optionsAction)
    {
        var options = new Tvision2Options();
        optionsAction?.Invoke(options);
        return builder.UseTvision2(options);
    }
    public static IHostBuilder UseTvision2(this IHostBuilder builder, Tvision2Options tv2Options)
    {
        builder.ConfigureServices(services =>
            {
                services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
                services.AddSingleton<Tvision2Options>(tv2Options);
                services.AddSingleton<Tvision2Engine>();
                services.AddHostedService<Tvision2EngineController>();
            })
            .UseConsoleLifetime();
        return builder;
    }
}