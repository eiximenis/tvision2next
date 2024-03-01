using Tvision2.Core;

namespace Tvision2.Styles.Builder;

public class StyleSetDefinition
{
    private Dictionary<string, StyleDefinition> _styles;

    public StyleSetDefinition()
    {
        _styles = new();
        _styles.Add("", new StyleDefinition());
    }
    

    public StyleDefinition WithDefaultStyle() => _styles[""];

    public StyleStateDefinition Default() => WithDefaultStyle().WithDefaultState();

    public StyleDefinition WithStyle(string name)
    {
        if (!_styles.ContainsKey(name))
        {
            _styles.Add(name, new StyleDefinition());
        }
        return _styles[name];
    } 
    
    
}