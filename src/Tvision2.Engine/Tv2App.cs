using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Engine.Extensions;

namespace Tvision2.Engine;

public static class Tv2App
{

    private static IHost? _host;

    public static async Task<IHost> Setup(IHostBuilder builder)
    {
        var host = builder.Build();
        await Setup(host);
        return _host;
    }

    public static async Task Setup(IHost host)
    {
        var engine = host.Services.GetService<Tvision2Engine>() ??
                     throw new InvalidOperationException(
                         "HostBuilder does not have Tvision2 enabled. Please call UseTvision2");
        
        _host = host;
    }
    
    public static async Task<IHost> Setup(Action<ITvision2Options>? optionsAction = null, Action<IHostBuilder>? additionalConfig = null)
    {   
        var builder = new HostBuilder().UseTvision2(optionsAction);
        additionalConfig?.Invoke(builder);
        return await Setup(builder);
    }
    
    public static async Task Run()
    {
        if (_host is null)
        {
            throw new InvalidOperationException("Setup must be called before running the application");
        }
        await _host.RunAsync();
    }
    
    public static ITvision2Engine  GetEngine() => 
        _host?.Services.GetRequiredService<Tvision2Engine>() as ITvision2Engine ??  throw new InvalidOperationException("Setup must be called before running the application");
}