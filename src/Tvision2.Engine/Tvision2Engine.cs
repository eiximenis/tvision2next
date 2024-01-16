using Tvision2.Console;
using Tvision2.Core.Console;
using Tvision2.Core.Engine.Render;
using Tvision2.Engine.Components;
using Tvision2.Engine.Components.Events;
using Tvision2.Engine.Render;

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
        var crows = System.Console.WindowHeight;
        var ccols = System.Console.WindowWidth;
        UI = new TvUiManager(new VirtualConsole(TvBounds.FromRowsAndCols(crows,ccols), TvColor.Black), _consoleDriver);
        UI.ComponentTree.Add(_options.BackgroundDefinition.CreateBackgroundComponent(), LayerSelector.Background);
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
        await UI.Update(false, new TvConsoleEvents());
        UI.Draw();
    }

    internal async Task Teardown()
    {
    }
}