using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Engine.Components.Extensions;

namespace Tvision2.Core.Engine;

public static class Tv2App
{

    private static IHost? _host;
    public static async Task<IHost> Setup(Action<ITvision2Options>? optionsAction = null)
    {   
        var options = new Tvision2Options();
        optionsAction?.Invoke(options);
        var builder = new HostBuilder().UseTvision2(options);
        _host = builder.Build();
        return _host;
    }
    
    public static async Task Run()
    {
        if (_host is null)
        {
            throw new InvalidOperationException("Setup must be called before running the application");
        }
        await _host.RunAsync();
    }
        
}