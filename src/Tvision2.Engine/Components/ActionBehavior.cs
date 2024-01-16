using Tvision2.Engine.Components;

namespace Tvision2.Core.Engine.Components;

class ActionBehavior<T> : ITvBehavior<T>
{
    private readonly Action<BehaviorContext<T>> _action;

    public ActionBehavior(Action<BehaviorContext<T>> action)
    {
        _action = action;
    }

    public void Do(BehaviorContext<T> context) => _action(context);
}