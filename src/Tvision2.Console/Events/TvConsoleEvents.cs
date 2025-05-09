using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Tvision2.Console.Events;

public interface ITvConsoleEventsSequences
{
    Span<TvConsoleKeyboardEvent> KeyboardEvents { get; }
    Span<TvConsoleMouseEvent> MouseEvents { get; }

    TvConsoleKeyboardEvent? AcquireFirstKeyboard(bool autoHandle) => AcquireFirstKeyboard(null, true);
    TvConsoleKeyboardEvent? AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool>? filter, bool autoHandle);

    TvWindowEvent? WindowEvent { get; }

    static ITvConsoleEventsSequences Empty => TvConsoleEventsEmpty.Instance;
}

class TvConsoleEventsEmpty : ITvConsoleEventsSequences
{
    public static TvConsoleEventsEmpty Instance { get; } = new ();
    public Span<TvConsoleKeyboardEvent> KeyboardEvents => Span<TvConsoleKeyboardEvent>.Empty;
    public Span<TvConsoleMouseEvent> MouseEvents => Span<TvConsoleMouseEvent>.Empty;

    public TvWindowEvent? WindowEvent => null;

    public TvConsoleKeyboardEvent? AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool>? filter, bool autoHandle) => null;

    

}

public class TvConsoleEvents : ITvConsoleEventsSequences
{

    /*
     * El mecanismo principal es el envío del signal SIGWINCH al proceso en primer plano.
       La aplicación debe manejar el signal SIGWINCH y luego utilizar ioctl(TIOCGWINSZ) para obtener las nuevas dimensiones de la ventana.
     */


    private readonly List<TvConsoleKeyboardEvent> _keyboardEvents;
    private readonly List<TvConsoleMouseEvent> _mouseEvents;
    private readonly TvWindowEvent _windowEvent;

    public TvConsoleEvents()
    {
        _keyboardEvents = new List<TvConsoleKeyboardEvent>(capacity: 10);
        _mouseEvents = new List<TvConsoleMouseEvent>(capacity: 10);
        _windowEvent = new TvWindowEvent(System.Console.WindowWidth, System.Console.WindowHeight);
    }

    public bool HasEvents => _keyboardEvents.Any(e => !e.IsHandled) || _mouseEvents.Any();
    public bool HasKeyboardEvents => _keyboardEvents.Any(e => !e.IsHandled);

    public bool HasWindowEvent => !_windowEvent.IsHandled;

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

    public void UpdateWindowSize(int newCols, int newRows)
    {
        if (newCols != _windowEvent.NewColumns || newRows != _windowEvent.NewRows)
        {
            _windowEvent.Update(newCols, newRows);
        }
    }

    public void Clear()
    {
        _mouseEvents.Clear();
        _keyboardEvents.Clear();
    }


    Span<TvConsoleKeyboardEvent> ITvConsoleEventsSequences.KeyboardEvents => CollectionsMarshal.AsSpan(_keyboardEvents);
    Span<TvConsoleMouseEvent> ITvConsoleEventsSequences.MouseEvents => CollectionsMarshal.AsSpan(_mouseEvents);
    public int Count => _keyboardEvents.Count + _mouseEvents.Count;

    public TvWindowEvent? WindowEvent => HasWindowEvent ? _windowEvent : null;
}
