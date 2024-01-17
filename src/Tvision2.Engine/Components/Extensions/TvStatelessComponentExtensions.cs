using Tvision2.Core;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Components.Extensions;

public static class TvStatelessComponentExtensions
{
public static  void AddDrawer(this TvComponent<Unit> component, Action<ConsoleContext> drawAction) => component.AddDrawer((ctx,_) => drawAction(ctx));
// public static void AddDrawer(this IDrawersAdder<Unit> adder, Action<RenderContext> drawAction) => adder.AddDrawer(new StatelessFuncDrawer(drawAction));

}