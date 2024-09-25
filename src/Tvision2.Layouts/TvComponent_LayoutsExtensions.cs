using System.Net.Sockets;
using Tvision2.Engine.Components;

namespace Tvision2.Layouts;

public static class TvComponent_LayoutsExtensions
{
    public static void DockTo(this TvComponent docked, TvComponent container, Dock dock)
    {
        docked.UseLayout(new DockLayout(container.Metadata, dock));
    }


}