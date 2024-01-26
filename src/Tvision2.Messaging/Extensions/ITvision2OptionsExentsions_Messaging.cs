using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Engine;

namespace Tvision2.Messaging.Extensions;

public static class ITvision2OptionsExentsions_Messaging
{
    public static IHostBuilder AddMessaging(this IHostBuilder builder)
    {
        builder.ConfigureServices(sp => sp.AddSingleton<MessageBus>());
        return builder;
    }
}

static class Tvision2EngineExtensions_Messaging
{
    public static MessageBus GetMessageBus() => Tv2App.GetEngine().GetRegisteredComponent<MessageBus>();
}
