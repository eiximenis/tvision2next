using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Engine.Render;

namespace Tvision2.Styles.Extensions
{
    public static class ConsoleContextExtensions_Styles
    {
        public static StyledConsoleContext StyledDefault(this ConsoleContext ctx, StylesManager stylesManager)
        {
            return new StyledConsoleContext(ctx, stylesManager.GetDefaultStyleSet());
        }

        public static StyledConsoleContext Styled(this ConsoleContext ctx, StylesManager stylesManager, string styleSetName)
        {
            return new StyledConsoleContext(ctx, stylesManager.GetStyleSet(styleSetName));
        }

    }
}
