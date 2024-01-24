using System.Runtime.InteropServices;

namespace Tvision2.Console.Events;

public interface ITvConsoleEventsSequences
{
    Span<TvConsoleKeyboardEvent> KeyboardEvents { get; }
    Span<TvConsoleMouseEvent> MouseEvents { get; }

    static ITvConsoleEventsSequences Empty => TvConsoleEventsEmpty.Instance;
}

class TvConsoleEventsEmpty : ITvConsoleEventsSequences
{
    public static TvConsoleEventsEmpty Instance { get; } = new ();

    public Span<TvConsoleKeyboardEvent> KeyboardEvents => Span<TvConsoleKeyboardEvent>.Empty;
    public Span<TvConsoleMouseEvent> MouseEvents => Span<TvConsoleMouseEvent>.Empty;
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

    public TvConsoleKeyboardEvent AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool> filter, bool autoHandle)
    {
        var evt = _keyboardEvents.Where(x => !x.IsHandled).FirstOrDefault(filter);
        if (autoHandle)
        {
            evt?.Handle();
        }
        return evt;
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
}
