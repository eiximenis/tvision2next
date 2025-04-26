using Tvision2.Core;
using Tvision2.Drawing.Borders;
using Tvision2.Engine.Render;

namespace Tvision2.Controls.Panels;


public class TvPanelState
{
    
    public BorderValue Border { get; set; }

    public TvPanelState(BorderValue border)
    {
        Border = border;
    }
}
public class TvPanel : TvEventedControl<TvPanelState>
{
    public TvPanel() : base(new TvPanelState(BorderValue.Single()))
    {
        Options.AutoSize = false;
        Component.AddDrawer(PanelDrawer);

    }

    private void PanelDrawer(ConsoleContext ctx, TvPanelState state)
    {
        var drawer = ctx.GetConsoleDrawer();
        BorderDrawer.Draw(drawer, state.Border, TvPoint.Zero, ctx.Viewzone.Bounds, TvColorsPair.FromForegroundAndBackground(TvColor.LightRed, TvColor.Black));
    }
}
