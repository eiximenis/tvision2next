using Microsoft.Extensions.Hosting;
using Tvision2.Engine.Components.Extensions;

namespace Tvision2.Core.Engine;

public class Tv2App
{
    public static async Task Run(Action<ITvision2Options>? optionsAction = null)
    {   
        var options = new Tvision2Options();
        optionsAction?.Invoke(options);
        var builder = new HostBuilder().UseTvision2(options);
        var host = builder.Build();
        await host.RunAsync();
    }
        
}