using Tvision2.Styles.Builder;

namespace Tvision2.Styles;

public interface ITvStylesBuilder
{
    StyleSetDefinition Default();
    StyleSetDefinition WithStyleSet(string name);
}