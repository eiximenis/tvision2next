using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Core.Engine;

namespace Tvision2.Engine.Components.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseTvision2(this IHostBuilder builder, Tvision2Options tv2options)
    {
        builder.ConfigureServices(services =>
            {
                services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
                services.AddSingleton<Tvision2Options>(tv2options);
                services.AddSingleton<Tvision2Engine>();
                // services.AddSingleton<Tvision2Engine>(sp => sp.GetRequiredService<Tvision2Engine>());
                services.AddHostedService<Tvision2EngineController>();
            })
            .UseConsoleLifetime();
        return builder;
    }
}