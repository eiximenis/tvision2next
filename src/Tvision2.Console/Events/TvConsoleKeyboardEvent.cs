namespace Tvision2.Console.Events;

public abstract class TvConsoleKeyboardEvent
{

    public bool IsHandled { get; private set; }

    protected TvConsoleKeyboardEvent()
    {
        IsHandled = false;
    }

    public void Handle() => IsHandled = true;

    public abstract ConsoleKeyInfo AsConsoleKeyInfo();
}