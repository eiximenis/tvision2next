namespace Tvision2.Console.Events;

public class AnsiConsoleKeyboardEvent : TvConsoleKeyboardEvent
{
    private readonly ConsoleKeyInfo _keyinfo;
        
    public AnsiConsoleKeyboardEvent(ConsoleKeyInfo info)
    {
        _keyinfo = info;
    }

    public override ConsoleKeyInfo AsConsoleKeyInfo() => _keyinfo;
}