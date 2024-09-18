using Tvision2.Core;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Components.Backgrounds;

public class BackgroundDefinition
{
    private ITvDrawer<Unit>? _drawer;
    private ITvBehavior<Unit>? _behavior;

    public BackgroundDefinition UseDrawer(ITvDrawer<Unit> drawer)
    {
        _drawer = drawer;
        return this;
    }

    public BackgroundDefinition UseDrawer(Action<ConsoleContext> drawerAction)
    {
        _drawer = new StatelessFuncDrawer<Unit>(drawerAction);
        return this;
    }

    public BackgroundDefinition UseBehavior(ITvBehavior<Unit> behavior)
    {
        _behavior = behavior;
        return this;
    }

    public BackgroundDefinition UseBehavior(Action<BehaviorContext<Unit>> behaviorFunc)
    {
        _behavior = new ActionBehavior<Unit>(behaviorFunc);
        return this;
    }

    internal TvComponent<Unit> CreateBackgroundComponent()
    {
        var bgComponent = TvComponent.CreateStatelessComponent(LayerSelector.Background, Viewports.FullViewport);
        if (_drawer is not null)
        {
            bgComponent.AddDrawer(_drawer);
        }

        if (_behavior is not null)
        {
            bgComponent.AddBehavior(_behavior);
        }

        return bgComponent;
    }
}

public static class DefaultBackgroundDefinitionsProvider
{
    public static BackgroundDefinition SolidColorBackground(TvColor bgColor) => 
        new BackgroundDefinition().UseDrawer(ctx => ctx.Fill(bgColor));
}