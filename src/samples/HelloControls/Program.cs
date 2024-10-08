using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Checkbox;
using Tvision2.Controls.Extensions;
using Tvision2.Controls.Extensions.Styles;
using Tvision2.Controls.Label;
using Tvision2.Controls.Layout.Grid;
using Tvision2.Controls.Panel;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Extensions;
using Tvision2.Layouts;
using Tvision2.Styles.Extensions;


var host = await Tv2App.Setup(
    o => o.AddConsoleOptions(c => c.UseAlternateBuffer()),
    hb =>
    {
        hb.AddTvControls();
        hb.AddStyles(s =>
        {
            s.Default().UseColors(TvColor.Yellow, TvColor.Blue);
            s.WithControlStyles().Default().WithControlState(ControlStyleState.Focused).UseColors(TvColor.Red, TvColor.Black);
            s.WithStyleSet("Alternate").Default().UseColors(TvColor.Red, TvColor.Black);
        });
    });


var app = host.Services.GetRequiredService<Tvision2Engine>();

int counter = 0;
var button = CreateButton("Click!", TvPoint.FromXY(1,1));
button.On().Tapped.Do(b => b.Text = $"Tapped {counter++}!");
// Can also use the alternate syntax:
// button.On(b => b.Tapped.Do(db => db.Text = $"Tapped {counter++}!"));
var label = new TvLabel("Check is unchecked");
var check = CreateCheckbox("Check me!", TvPoint.FromXY(1, 2));
check.On().CheckedChanged.Do(cb => label.Text = $"Checkbox is {cb.AsComponent().State.IsChecked}             ");


var panel = new TvPanel();
panel.AsComponent().Viewport.Resize(TvBounds.FromRowsAndCols(3, 10));
panel.MoveTo(TvPoint.FromXY(10, 20));

app.UI.ComponentTree.Add(button);
app.UI.ComponentTree.Add(check);
app.UI.ComponentTree.Add(label);
app.UI.ComponentTree.Add(panel);

var ctr = new TvPanel();
ctr.MoveTo(TvPoint.FromXY(2, 4));
ctr.Resize(TvBounds.FromRowsAndCols(10, 40));
var grid = new GridContainer(ctr.AsComponent().AsContainer(Margin.FromValue(1)), new GridDefinition());
button.Options.WithoutAutoSize();
button.AsComponent().DockTo(grid.At(1, 0), Dock.Top); 

var tvgrid = new TvGrid(new GridDefinition());
tvgrid.AsComponent().DockTo(grid.At(1, 1), Dock.Fill);
var gbutton = CreateButton("Grid", TvPoint.Zero);
gbutton.Options.WithoutAutoSize();
gbutton.AsComponent().DockTo(tvgrid.At(1, 1), Dock.Top);

app.UI.ComponentTree.Add(ctr);
app.UI.ComponentTree.Add(tvgrid);
app.UI.ComponentTree.Add(gbutton);

button.Focus();
await Tv2App.Run();

TvButton CreateButton(string text, TvPoint location)
{
    var button = new TvButton(text);

    button.MoveTo(location);
    return button;

}

TvCheckbox CreateCheckbox(string text, TvPoint location)
{
    var check = new TvCheckbox(text);
    check.MoveTo(location);
    return check;
}
