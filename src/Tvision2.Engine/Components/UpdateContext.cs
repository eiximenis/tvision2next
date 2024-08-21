using Tvision2.Console.Events;

namespace Tvision2.Engine.Components;

public readonly record struct UpdateContext(TvConsoleEvents ConsoleEvents, long LastElapsed);