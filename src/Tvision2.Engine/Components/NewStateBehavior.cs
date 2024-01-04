using Tvision2.Engine.Components;

namespace Tvision2.Core.Engine.Components;

class NewStateBehavior<T> :  ITvBehavior<T>
{
    private readonly Func<T, T> _getNewState;
    private readonly StateEqualityComparer _equalityComparer;
    public NewStateBehavior(Func<T, T> getNewState, StateEqualityComparer equalityComparer)
    {
        _getNewState = getNewState;
        _equalityComparer = equalityComparer;
    }

    public BehaviorResult<T> Do(in BehaviorContext<T> context)
    {
        var newState = _getNewState(context.State);
        var isSameState = _equalityComparer switch
        {
            StateEqualityComparer.ByReference => ReferenceEquals(newState, context.State),
            _ => context.State.Equals(newState)
        };
        return isSameState ? context.UnchangedResult() : context.NewStateResult(newState);
    }
}