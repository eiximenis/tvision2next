namespace Tvision2.Engine.Events;


public enum ActionResult
{
    Continue,
    Break
}

public interface IAction<TData>
{
    Task<ActionResult> Invoke(TData data);
}

public interface IActionsChain
{
    void Remove(Guid id);
    void Clear();
    IEnumerable<Guid> Keys { get; }
}

public interface IActionsChain<TData> : ISingleActionsChain<TData>
{
    Guid Do(IAction<TData> action);
    Guid Do(Action<TData> actionFunc);
    Guid Do(Func<TData, ActionResult> actionFunc);
    Guid Do(Func<TData, Task> actionFunc);
    Guid Do(Func<TData, Task<ActionResult>> actionFunc);
}

public class ActionsChain<TData> : IActionsChain<TData>
{
    private readonly Lazy<Dictionary<Guid, (IAction<TData> cmd, bool once)>> _guidedCommands;
    public IEnumerable<Guid> Keys => _guidedCommands.Value.Select(x => x.Key);

    public ActionsChain()
    {
        _guidedCommands = new Lazy<Dictionary<Guid, (IAction<TData>, bool)>>();
    }

    public async Task Invoke(TData item)
    {
        if (!_guidedCommands.IsValueCreated || !_guidedCommands.Value.Any())
        {
            return;
        }
        var guidsToDelete = new List<Guid>();
        var commands = _guidedCommands.Value;

        foreach (var guidedCommand in commands)
        {
            var stop = await guidedCommand.Value.cmd.Invoke(item);
            if (guidedCommand.Value.once)
            {
                guidsToDelete.Add(guidedCommand.Key);
            }

            if (stop == ActionResult.Break) break;
        }

        foreach (var guid in guidsToDelete)
        {
            Remove(guid);
        }
    }

    public Guid Do(Action<TData> actionFunc) =>
        Do(new DelegateAction<TData>(actionFunc));

    public Guid Do(Func<TData, ActionResult> actionFunc) =>
        Do(new DelegateAction<TData>(actionFunc));


    public Guid Do(Func<TData, Task> actionFunc) =>
        Do(new DelegateAction<TData>(actionFunc));

    public Guid Do(Func<TData, Task<ActionResult>> actionFunc) =>
        Do(new DelegateAction<TData>(actionFunc));

    public Guid Do(IAction<TData> action)
    {
        var guid = Guid.NewGuid();
        _guidedCommands.Value.Add(guid, (action, false));
        return guid;
    }


    public Guid DoOnce(IAction<TData> action)
    {
        var guid = Guid.NewGuid();
        _guidedCommands.Value.Add(guid, (action, true));
        return guid;
    }


    public Guid DoOnce(Action<TData> actionFunc) =>
        DoOnce(new DelegateAction<TData>(actionFunc));

    public Guid DoOnce(Func<TData, ActionResult> actionFunc) =>
        DoOnce(new DelegateAction<TData>(actionFunc));

    public Guid DoOnce(Func<TData, Task> actionFunc) =>
        DoOnce(new DelegateAction<TData>(actionFunc));

    public Guid DoOnce(Func<TData, Task<ActionResult>> actionFunc) =>
        DoOnce(new DelegateAction<TData>(actionFunc));

    public void Remove(Guid id)
    {
        if (_guidedCommands.Value.ContainsKey(id))
        {
            _guidedCommands.Value.Remove(id);
        }
    }

    public void Clear()
    {
        _guidedCommands.Value.Clear();
    }

}
