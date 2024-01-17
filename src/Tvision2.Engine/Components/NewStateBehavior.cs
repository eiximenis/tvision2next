namespace Tvision2.Engine.Components;

class NewStateBehavior<T> :  ITvBehavior<T>
{
    private readonly Func<T, T> _getNewState;
    private readonly StateEqualityComparer _equalityComparer;
    public NewStateBehavior(Func<T, T> getNewState, StateEqualityComparer equalityComparer)
    {
        _getNewState = getNewState;
        _equalityComparer = equalityComparer;
    }

    public void Do(BehaviorContext<T> context)
    {
        var newState = _getNewState(context.State);
        var isSameState = _equalityComparer switch
        {
            StateEqualityComparer.ByReference => ReferenceEquals(newState, context.State),
            _ => context.State.Equals(newState)
        };
        if (!isSameState) { context.ReplaceState(newState);}
    }
}