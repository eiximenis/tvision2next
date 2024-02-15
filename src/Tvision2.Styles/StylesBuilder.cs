using Tvision2.Styles.Builder;

namespace Tvision2.Styles;

class StylesBuilder : ITvStylesBuilder
{
    private readonly Dictionary<string, StyleSetDefinition> _styleSets;
    
    public ApplicationStyleSets Build() => new ApplicationStyleSets();

    public StylesBuilder()
    {
        _styleSets = new();
        _styleSets.Add("", new StyleSetDefinition());               // Default definition
    }

    StyleSetDefinition ITvStylesBuilder.Default() => _styleSets[""];
    
    StyleSetDefinition ITvStylesBuilder.WithStyleSet(string name)
    {
        if (!_styleSets.ContainsKey(name))
        {
            _styleSets.Add(name, new StyleSetDefinition());
        }

        return _styleSets[name];
    }
}