using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace SpaceInvaders;
internal static class Fps
{
    public static TvComponent<int> CreateComponent()
    {
        var component = new TvComponent<int>(0, Viewports.Null());
        component.AddDrawer(DrawFps);
        component.AddBehavior(CalculateFps);
        return component;
    }

    private static void DrawFps(ConsoleContext ctx, int state)
    {
        ctx.DrawStringAt(state.ToString(), TvPoint.Zero,
            TvColorsPair.FromForegroundAndBackground(TvColor.LightRed, TvColor.Black));
    }


    private static void CalculateFps(BehaviorContext<int> ctx)
    {
        var lastElapsed = ctx.LastElapsed;
        ctx.ReplaceState((int)(lastElapsed));
    }
}
