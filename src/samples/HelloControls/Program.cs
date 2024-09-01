﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Checkbox;
using Tvision2.Controls.Extensions;
using Tvision2.Controls.Extensions.Styles;
using Tvision2.Controls.Label;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Extensions;
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
var button = CreateButton("Click Me!", TvPoint.FromXY(1,1));
button.On().Tapped.Do(b => b.Text = $"Tapped {counter++}!");
// Can also use the alternate syntax:
// button.On(b => b.Tapped.Do(db => db.Text = $"Tapped {counter++}!"));
var check = CreateCheckbox("Check me!", TvPoint.FromXY(1, 2));

var label = new TvLabel("Check is unchecked");
app.UI.ComponentTree.Add(button);
app.UI.ComponentTree.Add(check);
app.UI.ComponentTree.Add(label);

check.On().CheckedChanged.Do(cb => label.Text = $"Checkbox is {cb.AsComponent().State.IsChecked}");

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
