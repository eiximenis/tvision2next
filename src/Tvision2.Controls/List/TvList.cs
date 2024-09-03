using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Drawing.Borders;
using Tvision2.Engine.Components;
using Tvision2.Engine.Drawing;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.List;
public class TvList : TvEventedControl<string>
{
    public TvList(string initialText = "") : base(initialText)
    {
        SetupComponent();
    }

    public TvList(TvComponent<string> existingComponent) : base(existingComponent)
    {
        SetupComponent();
    }

    private void SetupComponent()
    {
        Options.AutoSize = true;
        Component.AddStyledDrawer(ListDrawer, "TvControls");
        // Component.AddBehavior(AutoUpdateViewport);
    }

    private DrawResult ListDrawer(StyledConsoleContext ctx, string text)
    {

        return DrawResult.Done;
    }
}
