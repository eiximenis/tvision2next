using Microsoft.Extensions.DependencyInjection;
using SpaceInvaders;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Components.Backgrounds;
using Tvision2.Engine.Extensions;

var bgStars = new BrightingStarsBg();
var bg = new BackgroundDefinition().UseDrawer(bgStars.Draw).UseBehavior(bgStars.Update);
var host = await Tv2App.Setup(o =>
{
    o.WithBackground(bg);
    o.AddConsoleOptions(c => c.UseAlternateBuffer());
},
    hb => hb.ConfigureServices ((_, sc)  => sc.AddTvisionHook<Game>())
);

var app = host.Services.GetRequiredService<Tvision2Engine>();

var fps = Fps.CreateComponent();
fps.Viewport.MoveAndResize(TvPoint.Zero, TvBounds.FromRowsAndCols(1, 3));
app.UI.ComponentTree.Add(fps);

await Tv2App.Run(); 