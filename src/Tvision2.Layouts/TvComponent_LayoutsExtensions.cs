using System.Net.Sockets;
using Tvision2.Engine;
using Tvision2.Engine.Components;
using Tvision2.Engine.Layouts;

namespace Tvision2.Layouts;

public static class TvComponent_LayoutsExtensions
{
    public static void DockTo(this TvComponent docked, ITvContainer  container, Dock dock)
    {
        docked.UseLayout(new DockLayout(container, dock));
    }

    public static void DockToConsole(this TvComponent docked, Dock dock = Dock.Fill)    
    {
        var consoleConainer = Tv2App.GetEngine().GetRegisteredComponent<ConsoleContainer>();
        docked.UseLayout(new DockLayout(consoleConainer, dock));
    }

}