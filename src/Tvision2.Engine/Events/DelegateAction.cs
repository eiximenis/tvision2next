namespace Tvision2.Engine.Events;

public class DelegateAction<TData> : IAction<TData>
{
    private readonly Func<TData, Task<ActionResult>> _action;
    private readonly Func<TData, bool>? _predicate;

    public DelegateAction(Func<TData, Task<ActionResult>> action, Func<TData, bool>? predicate = null)
    {
        _action = action;
        _predicate = predicate;
    }

    public DelegateAction(Func<TData, ActionResult> action, Func<TData, bool>? predicate = null)
    {
        _action = d => Task.FromResult(action(d));
        _predicate = predicate;
    }

    public DelegateAction(Func<TData, Task> action, Func<TData, bool>? predicate = null)
    {
        _action = async d =>
        {
            await action(d);
            return ActionResult.Continue;
        };
        _predicate = predicate;
    }

    public DelegateAction(Action<TData> action, Func<TData, bool>? predicate = null)
    {
        _action = d =>
        {
            action(d);
            return Task.FromResult(ActionResult.Continue);
        };
        _predicate = predicate;
    }

    public async Task<ActionResult> Invoke(TData data)
    {
        if (_predicate == null || _predicate(data))
        {
            return await _action.Invoke(data);
        }
        return ActionResult.Continue;
    }
}