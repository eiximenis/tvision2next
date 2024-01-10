
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Engine.Components;

var host = await Tv2App.Setup(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()));
var app = host.Services.GetRequiredService<Tvision2Engine>();
var component = TvComponent.CreateStatelessComponent();
component.AddDrawer(ctx => ctx.DrawStringAt("Hello World!", TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Black)));
await app.UI.ComponentTree.Add(component);

System.Console.WriteLine("Doing things");

await Tv2App.Run();

