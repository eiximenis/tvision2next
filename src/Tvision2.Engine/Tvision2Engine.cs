using Tvision2.Core.Engine.Render;

namespace Tvision2.Core.Engine;

public class Tvision2Engine
{
    public bool Running { get; private set; } = false;
    private readonly Tvision2Options _options;
    public TvUiManager UI { get; }
    
    public Tvision2Engine(Tvision2Options options)
    {
        _options = options;
        UI = new TvUiManager(new VirtualConsole(TvBounds.FromRowsAndCols(24,80), TvColor.Black));
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