using Tvision2.Engine;

namespace Tvision2.Styles;

public class StylesManager
{
    private readonly ApplicationStyleSets _applicationStyleSets;
    public StylesManager(ApplicationStyleSets styleSets, Tvision2Engine engine)
    {
        _applicationStyleSets = styleSets;
        engine.UI.ComponentTree.AddSharedTag(this);
    }

    public StyleSet GetDefaultStyleSet() => _applicationStyleSets.DefaultSet;
    public StyleSet GetStyleSet(string styleSettName) => _applicationStyleSets.GetStyleSetOrDefault(styleSettName);
}