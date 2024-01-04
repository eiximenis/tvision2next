namespace Tvision2.Core.Engine;

public class Tvision2Engine
{
    public bool Running { get; private set; } = false;
    private readonly Tvision2Options _options;
    private readonly TvUiManager _uiManager;


    public Tvision2Engine(Tvision2Options options)
    {
        _options = options;
    }
    
    public async Task Initialize()
    {
        Running = true;
        await InvokeStartup();
    }

    private async Task InvokeStartup()
    {
        
    }

    public async Task NextCycle()
    {
    }

    public async Task Teardown()
    {
    }
}