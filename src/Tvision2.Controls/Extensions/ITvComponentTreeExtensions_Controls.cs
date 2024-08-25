using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Engine.Components;

namespace Tvision2.Controls.Extensions;
public static class ITvComponentTreeExtensions_Controls
{
    public static void Add(this ITvComponentTree tree, ITvControl control) => tree.Add(control.AsComponent());

    public static void Add(this ITvComponentTree tree, ITvControl control, LayerSelector layer) =>
        tree.Add(control.AsComponent(), layer);

    public static void AddChild(this ITvComponentTree tree, ITvControl child, ITvControl parent) =>
        tree.AddChild(child.AsComponent(), parent.AsComponent());
}
