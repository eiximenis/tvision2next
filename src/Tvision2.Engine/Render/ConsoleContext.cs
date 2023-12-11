namespace Tvision2.Core.Engine.Render;

public readonly struct ConsoleContext
{
    private readonly VirtualConsole _console;
    private readonly Viewport _viewport;
    public ConsoleContext(VirtualConsole console, Viewport viewport)
    {
        _console = console;
        _viewport = viewport;
    }
    
    
    private static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition) => viewPoint + viewportPosition;
    private static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition) => consolePoint - viewportPosition;
}