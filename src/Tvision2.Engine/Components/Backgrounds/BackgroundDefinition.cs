using Tvision2.Core;
using Tvision2.Core.Engine.Components;
using Tvision2.Core.Engine.Render;

namespace Tvision2.Engine.Components.Backgrounds;

public class BackgroundDefinition
{
    internal ITvDrawer<Unit> Drawer { get; private set; }

    public BackgroundDefinition UseDrawer(ITvDrawer<Unit> drawer)
    {
        Drawer = drawer;
        return this;
    }

    public BackgroundDefinition UseDrawer(Action<ConsoleContext> drawerAction)
    {
        Drawer = new StatelessFuncDrawer<Unit>(drawerAction);
        return this;
    }
    
    internal TvComponent<Unit> CreateBackgroundComponent()
    {
        var bgComponent = TvComponent.CreateStatelessComponent();
        bgComponent.AddDrawer(Drawer);
        bgComponent.UseViewport(Viewport.FullViewport);
        return bgComponent;
    }
}

public static class SolidBackgroundDefinitionProvider
{
    public static BackgroundDefinition SolidColorBackground(TvColor bgColor) => 
        new BackgroundDefinition().UseDrawer(ctx => ctx.Fill(bgColor));
}