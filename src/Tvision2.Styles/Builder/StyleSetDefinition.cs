namespace Tvision2.Styles.Builder;

public class StyleSetDefinition
{
    private Dictionary<string, StyleDefinition> _styles;

    public StyleSetDefinition()
    {
        _styles = new();
        _styles.Add("", new StyleDefinition());
    }
    
    public StyleDefinition WithStyleSet(string name)
    {
        if (!_styles.ContainsKey(name))
        {
            _styles.Add(name, new StyleDefinition());
        }
        return _styles[name];
    } 
    
    
}