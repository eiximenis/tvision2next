namespace Tvision2.Engine.Components;

/// <summary>
/// A BehaviorContext is passed to a behavior to allow access to state and a build factory for the BehaviorResult 
/// </summary>
public readonly struct BehaviorContext<T>(T initialState, T originalState)
{
    public T State { get; } = initialState;

    public T OriginalState { get; } = originalState;

    public BehaviorResult<T> UnchangedResult() => new BehaviorResult<T>(State, DirtyStatus.Clean);
    public BehaviorResult<T> DirtyResult() => new BehaviorResult<T>(State, DirtyStatus.CurrentStateChanged);
    public BehaviorResult<T> NewStateResult(T newState) => new BehaviorResult<T>(newState, DirtyStatus.NewState);
}