using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Controls;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Extensions;


var host = Tv2App.Setup(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()), hb=>hb.AddTvControls());
var app = host.Services.GetRequiredService<Tvision2Engine>();

var label = TvControl.Factory.CreateLabel("Hello World!");
label.Text = "Hello World! 2";
var button = TvControl.Factory.CreateButton("Click Me!");
button.MoveTo(TvPoint.FromXY(0, 1));
await app.UI.ComponentTree.Add(label.AsComponent());
await app.UI.ComponentTree.Add(button.AsComponent());
button.Focus();
await Tv2App.Run(); 