using Tvision2.Core;

namespace Tvision2.Engine.Components;

/// <summary>
/// A BehaviorContext is passed to a behavior to allow access to state and a build factory for the BehaviorResult 
/// </summary>
public class BehaviorContext<T>
{

    private readonly List<Func<TvComponent<T>, DirtyStatus>> _resultActions;
    
    private readonly TvComponent<T> _owner;
    public BehaviorContext(TvComponent<T> owner)
    {
        _owner = owner;
        _resultActions = new List<Func<TvComponent<T>, DirtyStatus>>();
    }

    public T  State => _owner.State;
    
    public void ReplaceState(T newState) 
    {
        _resultActions.Add( c =>
        {
            c.SetState(newState);
            return DirtyStatus.StateChanged;
        });
    }

    public void StatusUpdated()
    {
        _resultActions.Add(_ => DirtyStatus.StateChanged);
    }

    public void Resize(TvBounds newBounds)
    {
        _resultActions.Add(c =>
        {
            Viewport v = new Viewport(c.Viewport.Position, newBounds);
            c.UseViewport(v);
            return DirtyStatus.ViewportUpdated;
        });
    }

    public void Move(TvPoint position)
    {
        _resultActions.Add(c =>
        {
            Viewport v = new Viewport(position, c.Viewport.Bounds);
            c.UseViewport(v);
            return DirtyStatus.ViewportUpdated;
        });
    }

    internal DirtyStatus ApplyChanges()
    {
        var status = DirtyStatus.Clean;
        foreach (var action in _resultActions)
        {
            status |= action(_owner);
        }

        _resultActions.Clear();
        return status;
    }
}