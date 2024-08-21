using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Tvision2.Console.Events;

public interface ITvConsoleEventsSequences
{
    Span<TvConsoleKeyboardEvent> KeyboardEvents { get; }
    Span<TvConsoleMouseEvent> MouseEvents { get; }

    TvConsoleKeyboardEvent? AcquireFirstKeyboard(bool autoHandle) => AcquireFirstKeyboard(null, true);
    TvConsoleKeyboardEvent? AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool>? filter, bool autoHandle);

    static ITvConsoleEventsSequences Empty => TvConsoleEventsEmpty.Instance;
}

class TvConsoleEventsEmpty : ITvConsoleEventsSequences
{
    public static TvConsoleEventsEmpty Instance { get; } = new ();

    public Span<TvConsoleKeyboardEvent> KeyboardEvents => Span<TvConsoleKeyboardEvent>.Empty;
    public Span<TvConsoleMouseEvent> MouseEvents => Span<TvConsoleMouseEvent>.Empty;

    private bool HasEvents { get; } = false;
    private bool HasKeyboardEvent { get; } = false;
    private int Count { get; } = 0;


    public TvConsoleKeyboardEvent? AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool>? filter, bool autoHandle) => null;

}

public class TvConsoleEvents : ITvConsoleEventsSequences
{
    private readonly List<TvConsoleKeyboardEvent> _keyboardEvents;
    private readonly List<TvConsoleMouseEvent> _mouseEvents;

    public TvConsoleEvents()
    {
        _keyboardEvents = new List<TvConsoleKeyboardEvent>(capacity: 10);
        _mouseEvents = new List<TvConsoleMouseEvent>(capacity: 10);
    }

    public bool HasEvents => _keyboardEvents.Any(e => !e.IsHandled) || _mouseEvents.Any();
    public bool HasKeyboardEvents => _keyboardEvents.Any(e => !e.IsHandled);

    public TvConsoleKeyboardEvent? AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool>? filter, bool autoHandle)
    {
        for (var idx = 0; idx < _keyboardEvents.Count; idx++)
        {
            var evt = _keyboardEvents[idx];
            if (!evt.IsHandled)
            {
                if (filter is null || filter(evt))
                {
                    if (autoHandle)
                    {
                        evt.Handle();
                    }

                    return evt;
                }
            }
        }

        return null;
    }
    
    public void Add(TvConsoleKeyboardEvent @event) => _keyboardEvents.Add(@event);
    public void Add(TvConsoleMouseEvent @event) => _mouseEvents.Add(@event);

    public void Clear()
    {
        _mouseEvents.Clear();
        _keyboardEvents.Clear();
    }

    Span<TvConsoleKeyboardEvent> ITvConsoleEventsSequences.KeyboardEvents => CollectionsMarshal.AsSpan(_keyboardEvents);
    Span<TvConsoleMouseEvent> ITvConsoleEventsSequences.MouseEvents => CollectionsMarshal.AsSpan(_mouseEvents);
    public int Count => _keyboardEvents.Count + _mouseEvents.Count;
}
