using System.Xml.Linq;
using Tvision2.Styles.Builder;

namespace Tvision2.Styles;

class StylesBuilder : ITvStylesBuilder
{
    private readonly Dictionary<string, StyleSetDefinition> _styleSets;
    
    public StylesBuilder()
    {
        _styleSets = new();
        _styleSets.Add("", new StyleSetDefinition(""));

    }

    StyleSetDefinition ITvStylesBuilder.DefaultStyleSet() => _styleSets[""];


    StyleDefinition ITvStylesBuilder.WithStyle(string name) => ((ITvStylesBuilder)this).DefaultStyleSet().WithStyle(name);

    StyleDefinition ITvStylesBuilder.WithDefaultStyle() => ((ITvStylesBuilder)this).DefaultStyleSet().WithDefaultStyle();

    StyleStateDefinition ITvStylesBuilder.Default() => ((ITvStylesBuilder)this).DefaultStyleSet().WithDefaultStyle().WithDefaultState();

    StyleSetDefinition ITvStylesBuilder.WithStyleSet(string name)
    {
        if (!_styleSets.ContainsKey(name))
        {
            _styleSets.Add(name, new StyleSetDefinition(name));
        }

        return _styleSets[name];
    }


    public ApplicationStyleSets Build()
    {
        var appStyles = new ApplicationStyleSets();

        foreach (var styleSetDefinition in _styleSets.Values) 
        {
            var styleSet = styleSetDefinition.ToStyleSet();
            appStyles.AddStyleSet(styleSet);
        }

        return appStyles;
    }

}