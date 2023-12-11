namespace Tvision2.Engine.Components;

public readonly struct BehaviorContext<T>
{
    public T State { get; }

    public T OriginalState { get; }

    public BehaviorContext(T initialState, T originalState)
    {
        State = initialState;
        OriginalState = originalState;
    }

    public BehaviorResult<T> UnchangedResult() => new BehaviorResult<T>(State, DirtyStatus.Clean);
    public BehaviorResult<T> DirtyResult() => new BehaviorResult<T>(State, DirtyStatus.CurrentStateChanged);
    public BehaviorResult<T> NewStateResult(T newState) => new BehaviorResult<T>(newState, DirtyStatus.NewState);
}