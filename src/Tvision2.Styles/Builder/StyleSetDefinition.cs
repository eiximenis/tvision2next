using Tvision2.Core;

namespace Tvision2.Styles.Builder;

public class StyleSetDefinition
{
    private Dictionary<string, StyleDefinition> _styles;
    private readonly string _name;

    public StyleSetDefinition(string name)
    {
        _name = name;
        _styles = new();
        _styles.Add("", new StyleDefinition(""));
    }
    

    public StyleDefinition WithDefaultStyle() => _styles[""];

    public StyleStateDefinition Default() => WithDefaultStyle().WithDefaultState();

    public StyleDefinition WithStyle(string name)
    {
        if (!_styles.ContainsKey(name))
        {
            _styles.Add(name, new StyleDefinition(name));
        }
        return _styles[name];
    } 

    
    internal StyleSet ToStyleSet()
    {
        var styleDefs = _styles.Values;

        var set = new StyleSet(_name, null);

        foreach (var styleDef in styleDefs)
        {
            set.Add(styleDef.ToStyle());
        }

        // Need to set the parents accordingly
        foreach (var styleDef in styleDefs)
        {
            if (styleDef.ParentName is not null)
            {
                set[styleDef.Name].SetParent(set[styleDef.ParentName]);
            }
        }

        return set;
    }
    
    
    
    
}