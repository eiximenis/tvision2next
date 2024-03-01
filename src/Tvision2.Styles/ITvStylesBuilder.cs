using Tvision2.Styles.Builder;

namespace Tvision2.Styles;

public interface ITvStylesBuilder
{
    StyleSetDefinition DefaultStyleSet();
    StyleSetDefinition WithStyleSet(string name);


    StyleStateDefinition Default();

    StyleDefinition WithStyle(string name);
    StyleDefinition WithDefaultStyle();

}