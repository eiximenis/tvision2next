using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using Tvision2.Console.Events;
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
        Events = ITvConsoleEventsSequences.Empty;
        _resultActions = new List<Func<TvComponent<T>, DirtyStatus>>();
    }
    
    public ITvConsoleEventsSequences Events { get; private set; }
    
    public T  State => _owner.State;
    public long LastElapsed { get; private set; }
    public ITvComponentTree ComponentTree => _owner.Metadata.OwnerTree!;

    internal void SetData(ITvConsoleEventsSequences events, long lastElapsed)
    {
        Events = events;
        LastElapsed = lastElapsed;
    }

    public TvBounds Bounds => _owner.Viewport.Bounds;
    
    public void ReplaceState(T newState) 
    {
        _resultActions.Add( c =>
        {
            c.SetState(newState);
            return DirtyStatus.StateChanged;
        });
    }

    public void StateChanged()
    {
        _resultActions.Add(_ => DirtyStatus.StateChanged);
    }

    public void Resize(TvBounds newBounds)
    {
        _resultActions.Add(c =>
        {
            c.Viewport.Resize(newBounds);
            return DirtyStatus.ViewportUpdated;
        });
    }

    public void Move(TvPoint position)
    {
        _resultActions.Add(c =>
        {
            c.Viewport.MoveTo(position);
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