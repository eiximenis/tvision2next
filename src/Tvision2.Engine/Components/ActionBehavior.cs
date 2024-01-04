using Tvision2.Engine.Components;

namespace Tvision2.Core.Engine.Components;

class ActionBehavior<T> : ITvBehavior<T>
{
    private readonly Func<BehaviorContext<T>, BehaviorResult<T>> _action;

    public ActionBehavior(Func<BehaviorContext<T>, BehaviorResult<T>> action)
    {
        _action = action;
    }

    public BehaviorResult<T> Do(in BehaviorContext<T> context)
    {
        return _action(context);
    }
}