
using Tvision2.Core.Engine;
using Tvision2.Core.Engine.Components;

await Tv2App.Run(o => o.AddConsoleOptions(c => c.UseAlternateBuffer()));



var component = TvComponent.CreateStatelessComponent();

