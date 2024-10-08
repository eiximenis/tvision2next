
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Components;
using Tvision2.Engine.Components.Backgrounds;
using Tvision2.Engine.Extensions;
using Tvision2.Engine.Layouts;
using Tvision2.Layouts;

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

var host = await Tv2App.Setup(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()).WithBackground(background));
var app = host.Services.GetRequiredService<Tvision2Engine>();
var component = TvComponent.CreateStatelessComponent();
var container =  TvComponent.CreateStatelessComponent(new Viewport(TvPoint.FromXY(3, 4), TvBounds.FromRowsAndCols(10, 40)));
container.AddDrawer(ctx => ctx.Fill(TvColor.Cyan));
container.AddBehavior(ctx =>
{
    var col = new Random().Next(0, 40);
    var row = new Random().Next(0, 20);
    ctx.Move(TvPoint.FromXY(col, row));
});
app.UI.ComponentTree.Add(container);

var component2 = TvComponent.CreateStatelessComponent(new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(1, 14)));
component2.AddDrawer(ctx => ctx.DrawStringAt("Hello World!", TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.LightBlack)));
// component2.DockTo(container.AsContainer(), Dock.Top);

component2.AttachWithMargin(container.AsContainer(), Margin.TopMargin(2));

app.UI.ComponentTree.Add(component2);

System.Console.WriteLine("Doing things");

await Tv2App.Run();

