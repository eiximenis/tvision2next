namespace Tvision2.Core.Engine.Events;

public interface ISingleActionsChain<TData> : IActionsChain
{
    Guid DoOnce(IAction<TData> action);
    Guid DoOnce(Action<TData> actionFunc);
    Guid DoOnce(Func<TData, ActionResult> actionFunc);
    Guid DoOnce(Func<TData, Task> actionFunc);
    Guid DoOnce(Func<TData, Task<ActionResult>> actionFunc);
}