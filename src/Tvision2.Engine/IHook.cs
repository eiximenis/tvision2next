using System.Diagnostics;
using Tvision2.Console.Events;

namespace Tvision2.Engine;

public interface IHook
{
    Task Init() => Task.CompletedTask;
    Task BeforeUpdate(TvConsoleEvents events);
    Task Teardown() => Task.CompletedTask;
}