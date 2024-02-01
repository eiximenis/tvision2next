using System.Diagnostics;
using Tvision2.Console.Events;

namespace Tvision2.Engine;

public interface IHook
{
    Task BeforeUpdate(TvConsoleEvents events);
}