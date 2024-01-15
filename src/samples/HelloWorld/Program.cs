
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Engine.Components;
using Tvision2.Engine.Extensions;

var host = Tv2App.Setup(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()));
Tv2App.Configure(opt => opt.WithBackground(TvColor.Red));
var app = host.Services.GetRequiredService<Tvision2Engine>();
var component = TvComponent.CreateStatelessComponent();
component.UseViewport(Viewport.FullViewport);
component.AddDrawer(ctx => ctx.DrawStringAt("Hello World!", TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Black)));
await app.UI.ComponentTree.Add(component);

System.Console.WriteLine("Doing things");

await Tv2App.Run();

