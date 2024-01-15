using Tvision2.Console;
using Tvision2.Core.Console;
using Tvision2.Core.Engine.Render;
using Tvision2.Engine.Components;

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
        UI.ComponentTree.Add(_options.BackgroundDefinition.CreateBackgroundComponent(), LayerSelector.Bottom);
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