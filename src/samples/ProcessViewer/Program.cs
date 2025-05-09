using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Controls.List;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Controls.Extensions;
using Tvision2.Styles.Extensions;
using Tvision2.Controls.Panels;
using Tvision2.Layouts;

var host = await Tv2App.Setup(
    o => o.AddConsoleOptions(c => c.UseAlternateBuffer()).WithBackgroundColor(TvColor.Red),
    hb => hb.AddTvControls().AddStyles());

var engine = Tv2App.GetEngine();

// Create the main Layout

var panel = new TvPanel();
panel.AsComponent().DockToConsole();
engine.UI.ComponentTree.Add(panel);
//panel.AsComponent().DockToConsole();


//list.MoveTo(TvPoint.FromXY(5, 5));  
//list.Resize(TvBounds.FromRowsAndCols(7, 10));

//engine.UI.ComponentTree.Add(list);

await Tv2App.Run();
