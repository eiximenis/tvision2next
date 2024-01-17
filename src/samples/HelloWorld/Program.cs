
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Components;
using Tvision2.Engine.Components.Backgrounds;
using Tvision2.Engine.Extensions;
using Tvision2.Engine.Layout;

string _text = "Hello World!";

var host = Tv2App.Setup(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()));
var background = new BackgroundDefinition().UseDrawer(ctx =>
{
    var bounds = ctx.Viewzone.Bounds;
    for (var row = 0; row <= bounds.Height; row++)
    {
        for (var col = 0; col <= bounds.Width; col++)
        {
            ctx.DrawStringAt(".", TvPoint.FromXY(col, row),
                TvColorsPair.FromForegroundAndBackground(TvColor.Black, TvColor.FromRgb(row + 30, col + 30, 0x100)));
        }
    }
});
Tv2App.Configure(opt => opt.WithBackground(background));
var app = host.Services.GetRequiredService<Tvision2Engine>();
var component = TvComponent.CreateStatelessComponent();
component.UseLayout(LayoutManagers.Blocked(TvPoint.FromXY(3, 3), TvBounds.FromRowsAndCols(1, 4)));
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

