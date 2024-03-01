using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls.Extensions.Styles;
using Tvision2.Styles.Builder;

namespace Tvision2.Controls.Extensions
{
    public static class StyleDefinitionExtensions_Controls
    {
        public static StyleStateDefinition WithControlState(this StyleDefinition styleDefition, ControlStyleState state)
        {
            return state switch
            {
                ControlStyleState.Normal => styleDefition.WithState("Normal"),
                _ => styleDefition.WithState("Focused")
            };
        }
    }
}
