using Microsoft.Extensions.DependencyInjection;
using Tvision2.Controls;
using Tvision2.Controls.Extensions;
using Tvision2.Engine;

var host = Tv2App.Setup(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()));
var app = host.Services.GetRequiredService<Tvision2Engine>();

var label = TvControl.Factory.CreateLabel("Hello World!");
label.Text = "Hello World! 2";
await app.UI.ComponentTree.Add(label.AsComponent());
await Tv2App.Run();