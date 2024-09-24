using Tvision2.Core;
using Tvision2.Drawing;

namespace Tvision2.Styles.Builder
{
    public class StyleStateDefinition
    {

        private StyleState? _builtState;
        private readonly string _name;

        public StyleStateDefinition(string name) => _name = name;


        public void UseColors(TvColor fore, TvColor back)
        {
            _builtState = new StyleState(_name, SolidDynamicColor.FromColor(fore), SolidDynamicColor.FromColor(back));
        }

        public void UseColors(TvColor fore, Func<IDynamicColor> back)
        {
            _builtState = new StyleState(_name, SolidDynamicColor.FromColor(fore), back());
        }

        public void UseColors(Func<IDynamicColor> fore, TvColor back)
        {
            _builtState = new StyleState(_name, fore(), SolidDynamicColor.FromColor(back));
        }


        public void UseColors(Func<IDynamicColor> fore, Func<IDynamicColor> back)
        {
            _builtState = new StyleState(_name, fore(), back());
        }

        internal StyleState BuiltState => _builtState!;


    }
}