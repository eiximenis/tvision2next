using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;

namespace Tvision2.Engine.Layouts;
public static class TvComponentBasicLayoutsExtensions
{
    public static void Attach(this TvComponent attached, ITvContainer container) =>
        attached.UseLayout(new RelativeLayoutManager(container, null));

    public static void AttachWithMargin(this TvComponent attached, ITvContainer container, Margin margin) =>
        attached.UseLayout(new RelativeLayoutManager(container, new MarginViewportSnapshotTransformer(margin)));
}
