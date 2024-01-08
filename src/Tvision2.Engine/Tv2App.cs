using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Engine.Components.Extensions;

namespace Tvision2.Core.Engine;

public static class Tv2App
{

    private static IHost? _host;
    public static async Task<Tvision2Engine> Start(Action<ITvision2Options>? optionsAction = null)
    {   
        var options = new Tvision2Options();
        optionsAction?.Invoke(options);
        var builder = new HostBuilder().UseTvision2(options);
        _host = builder.Build();
        await _host.RunAsync();
        return _host.Services.GetRequiredService<Tvision2Engine>();
    }
    
    public  static Task End()
    {
        System.Console.Write("Ending...");
        return Task.CompletedTask;
    }
        
}