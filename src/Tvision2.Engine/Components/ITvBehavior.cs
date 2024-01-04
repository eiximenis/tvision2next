namespace Tvision2.Engine.Components;

/// <summary>
/// Dirty Status for a Component
/// </summary>
public enum DirtyStatus
{
    Clean,      // Component is clean: No state changes
    CurrentStateChanged,       // Dirty because current state has changed
    NewState                   // Dirty because state has been replaced by a new state
}

/// <summary>
/// The result of a behavior executed
/// If DirtyStatus is NewState then State should have new state object. Otherwise State should contain the
///     the current State of the component. 
/// </summary>
public readonly record struct BehaviorResult<T>(T State, DirtyStatus DirtyStatus)
{
    public bool IsDirty => DirtyStatus != DirtyStatus.Clean;
}

/// <summary>
/// A Behavior that processes a State of type T
/// </summary>
public interface ITvBehavior<T>
{
    BehaviorResult<T> Do(in BehaviorContext<T> context);
}

