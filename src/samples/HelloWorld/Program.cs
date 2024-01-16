
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Engine.Components;
using Tvision2.Engine.Components;
using Tvision2.Engine.Extensions;

string _text = "Hello World!";

var host = Tv2App.Setup(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()));
Tv2App.Configure(opt => opt.WithBackground(TvColor.Red));
var app = host.Services.GetRequiredService<Tvision2Engine>();
var component = TvComponent.CreateStatelessComponent();
component.UseViewport(Viewport.FullViewport);
component.AddDrawer(ctx => ctx.DrawStringAt(_text, TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Black)));
component.AddBehavior(ctx =>
{
    var num = new Random().Next(1, 20);
    _text = "Hello World " + new string('!', num);
    ctx.Resize(TvBounds.FromRowsAndCols(1, _text.Length));
    var col = new Random().Next(0, 40);
    var row = new Random().Next(0, 20);
    ctx.Move(TvPoint.FromXY(col, row));
});
await app.UI.ComponentTree.Add(component);

System.Console.WriteLine("Doing things");

await Tv2App.Run();

