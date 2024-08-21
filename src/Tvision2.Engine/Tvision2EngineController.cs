using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Tvision2.Engine;

public class Tvision2EngineController : BackgroundService
{
    private readonly Tvision2Engine _engine;
    public Tvision2EngineController(Tvision2Engine engine)
    {
        _engine = engine;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _engine.Initialize();
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var running = _engine.Running;
        var stopwatch = new Stopwatch();
        var frametimeMs = 1000 / 30;
        var elapsed = 0L;

        while (!stoppingToken.IsCancellationRequested && running)
        {
            stopwatch.Start();
            await _engine.NextCycle(elapsed);
            stopwatch.Stop();
            elapsed = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            if (elapsed < frametimeMs)
            {
                await Task.Delay((int)(frametimeMs - elapsed));
            }
        }
        await _engine.Teardown();

    }
}