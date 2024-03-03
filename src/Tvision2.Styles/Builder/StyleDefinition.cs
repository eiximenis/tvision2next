using System.Xml.Linq;
using Tvision2.Core;

namespace Tvision2.Styles.Builder;

public class StyleDefinition
{

    private Dictionary<string, StyleStateDefinition> _states = new();

    private readonly string _name;

    public StyleDefinition(string name)
    {
        _name = name;
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

    internal Style ToStyle()
    {
        var style = new Style(_name);

        foreach (var stateDef in _states.Values)
        {
            style.Add(stateDef.BuiltState);
        }

        return style;
    }
    
}