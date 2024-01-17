using Tvision2.Core;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Components;

class StatelessFuncDrawer<T> : ITvDrawer<T>
{
    private readonly Action<ConsoleContext> _renderAction;
    public StatelessFuncDrawer(Action<ConsoleContext> renderAction)
    {
        _renderAction = renderAction;
    }
    public void Draw(in ConsoleContext context, T _) => _renderAction(context);

}

class FuncDrawer<T> : ITvDrawer<T> 
{
    private readonly Action<ConsoleContext, T> _renderAction;
    public FuncDrawer(Action<ConsoleContext, T> renderAction)
    {
        _renderAction = renderAction;
    }

    public void Draw(in ConsoleContext context, T state) => _renderAction(context, state);
}
