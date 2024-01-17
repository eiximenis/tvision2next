namespace Tvision2.Engine.Components;

class ActionBehavior<T> : ITvBehavior<T>
{
    private readonly Action<BehaviorContext<T>> _action;

    public ActionBehavior(Action<BehaviorContext<T>> action)
    {
        _action = action;
    }

    public void Do(BehaviorContext<T> context) => _action(context);
}