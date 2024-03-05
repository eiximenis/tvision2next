using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Styles.Extensions
{
    public abstract class StyledDrawer<T> : ITvDrawer<T>, IStyledDrawer<T>
    {

        private readonly TvComponent<T> _owner;
        private readonly string _styleSetName;
        protected StyledDrawer(TvComponent<T> component, string? styleSetName = null)
        {
            _owner = component;
            _styleSetName = styleSetName ?? "";
        }

        public DrawResult Draw(in ConsoleContext context, T data)
        {
            var stylesManager = _owner.Metadata.OwnerTree?.GetSharedTag<StylesManager>();

            if (stylesManager is null)
            {
                return DrawResult.Done;
            }

            var styledContext = context.Styled(stylesManager, _styleSetName);

            return Draw(in styledContext, data);
        }

        public abstract DrawResult Draw(in StyledConsoleContext context, T data);

        ITvDrawer<T> IStyledDrawer<T>.ToStandardDrawer() => this;
    }
}
