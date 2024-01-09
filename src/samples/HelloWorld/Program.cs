
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core.Engine;
using Tvision2.Core.Engine.Components;

var host = await Tv2App.Setup(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()));
var app = host.Services.GetRequiredService<Tvision2Engine>();
var component = TvComponent.CreateStatelessComponent();
await app.UI.ComponentTree.Add(component);

System.Console.WriteLine("Doing things");

await Tv2App.Run();

