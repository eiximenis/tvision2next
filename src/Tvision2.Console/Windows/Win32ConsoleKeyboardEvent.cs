using Tvision2.Console.Events;
using Tvision2.Console.Windows.Interop;

namespace Tvision2.Console.Windows;

class Win32ConsoleKeyboardEvent : TvConsoleKeyboardEvent
{
    private readonly Types.KEY_EVENT_RECORD _record;
    public Win32ConsoleKeyboardEvent(Types.KEY_EVENT_RECORD record)
    {
        _record = record;
    }

    public override ConsoleKeyInfo AsConsoleKeyInfo() => AsConsoleKeyInfo(_record);

    static ConsoleKeyInfo AsConsoleKeyInfo(Types.KEY_EVENT_RECORD record)
    {
        var (ctrl, alt, shift) = record.dwControlKeyState.GetModifiers();

        return new ConsoleKeyInfo(record.UnicodeChar, (ConsoleKey)record.wVirtualKeyCode, shift, alt, ctrl);

    }
}