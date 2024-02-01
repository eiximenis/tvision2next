using Microsoft.Extensions.DependencyInjection;

namespace Tvision2.Engine.Extensions;

public static class IServiceCollectionExtensions_Engine
{
    public static IServiceCollection AddTvisionHook<T>(this IServiceCollection services) where T : class, IHook
    {
        services.AddSingleton<IHook, T>();
        return services;
    } 
}