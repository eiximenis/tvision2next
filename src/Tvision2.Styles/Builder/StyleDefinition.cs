using System.Xml.Linq;
using Tvision2.Core;

namespace Tvision2.Styles.Builder;

public class StyleDefinition
{

    private Dictionary<string, StyleStateDefinition> _states = new();

    public StyleDefinition()
    {
        var defaultStateDef = new StyleStateDefinition("");
        defaultStateDef.UseColors(TvColor.White, TvColor.Black);
        _states.Add("", defaultStateDef);
    }

    public StyleStateDefinition WithState(string stateName)
    {
        if (!_states.ContainsKey(stateName))
        {
            _states.Add(stateName, new StyleStateDefinition(stateName));
        }

        return _states[stateName];
    }

    public StyleStateDefinition WithDefaultState() => _states[""];
    
}