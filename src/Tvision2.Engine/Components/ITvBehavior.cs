namespace Tvision2.Engine.Components;

/// <summary>
/// Dirty Status for a Component
/// </summary>
[Flags]
public enum DirtyStatus
{
    Clean = 0x0,              
    StateChanged = 0x1,       
    ViewportUpdated = 0x2                      
}


/// <summary>
/// A Behavior that processes a State of type T
/// </summary>
public interface ITvBehavior<T>
{
    void Do(BehaviorContext<T> context);
}

