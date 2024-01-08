
using Tvision2.Core.Engine;
using Tvision2.Core.Engine.Components;

var app = await Tv2App.Start(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()));
var component = TvComponent.CreateStatelessComponent();
System.Console.WriteLine("Doing things");
await Tv2App.End();

