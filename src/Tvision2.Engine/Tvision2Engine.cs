using Tvision2.Console;
using Tvision2.Core.Console;
using Tvision2.Core.Engine.Render;

namespace Tvision2.Core.Engine;

public class Tvision2Engine
{
    public bool Running { get; private set; } = false;
    private readonly Tvision2Options _options;
    private readonly IConsoleDriver _consoleDriver;
    public TvUiManager UI { get; }
    
    public Tvision2Engine(Tvision2Options options)
    {
        _options = options;
        _consoleDriver = new AnsiConsoleDriver(_options.ConsoleOptions);
        UI = new TvUiManager(new VirtualConsole(TvBounds.FromRowsAndCols(24,80), TvColor.Black), _consoleDriver);
    }
    
    public async Task Initialize()
    {
        Running = true;
        await InvokeStartup();
    }

    private async Task InvokeStartup()
    {
        
    }

    internal async Task NextCycle()
    {
        UI.Draw();
    }

    internal async Task Teardown()
    {
    }
}