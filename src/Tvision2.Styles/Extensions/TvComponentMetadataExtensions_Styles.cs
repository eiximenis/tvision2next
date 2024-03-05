using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Styles.Extensions
{
    public static class TvComponentMetadataExtensions_Styles
    {
        public static StyledConsoleContext GetStyledDefaultConsoleContext(this TvComponentMetadata componentMetadata, ConsoleContext ctx)
        {
            if (componentMetadata.OwnerTree is null)
            {
                throw new InvalidOperationException("Component is not attached to any tree!");
            }

            var stylesManager = componentMetadata.OwnerTree.GetSharedTag<StylesManager>();
            return ctx.StyledDefault(stylesManager!);

        }

        public static StyledConsoleContext GetStyledConsoleContext(this TvComponentMetadata componentMetadata, ConsoleContext ctx, string stylesSetName)
        {
            if (componentMetadata.OwnerTree is null)
            {
                throw new InvalidOperationException("Component is not attached to any tree!");
            }

            var stylesManager = componentMetadata.OwnerTree.GetSharedTag<StylesManager>();
            return ctx.Styled(stylesManager!, stylesSetName);
        }

    }
}
