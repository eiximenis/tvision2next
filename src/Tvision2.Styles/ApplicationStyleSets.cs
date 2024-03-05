namespace Tvision2.Styles;

public class ApplicationStyleSets
{

    private readonly Dictionary<string, StyleSet> _syleSets = new();

    public StyleSet DefaultSet => _syleSets[""];


    public StyleSet GetStyleSetOrDefault(string styleSetName) => _syleSets.TryGetValue(styleSetName, out var styleSet) ? styleSet : DefaultSet;
    

    public void AddStyleSet(StyleSet styleSet)
    {
        if (_syleSets.ContainsKey(styleSet.Name))
        {
            throw new InvalidOperationException($"StyleSet {styleSet.Name} already exists!");
        }

        _syleSets.Add(styleSet.Name, styleSet);
    }
    
}