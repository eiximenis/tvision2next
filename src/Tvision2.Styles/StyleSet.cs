namespace Tvision2.Styles;

public class StyleSet
{
    private readonly Dictionary<string, Style> _styles;
    private Style? _defaultStyle;
    public const string DefaultStyleName = "default";
    public string Name { get; }
    public IEnumerable<Style> OwnedStyles => _styles.Values;
    public IEnumerable<Style> AllStyles => _styles.Values.Union(_parent?.AllStyles ?? Enumerable.Empty<Style>());

    private StyleSet? _parent;

    public StyleSet(string name, StyleSet? parent)
    {
        _styles = new Dictionary<string, Style>();
        _defaultStyle = null;
        _parent = parent;
        Name = name;        
    }
    
    internal void Add(Style style)
    {
        _styles.Add(style.Name, style);
        if (style.Name == DefaultStyleName || _styles.Count == 1)
        {
            _defaultStyle = style;
        }
    }
    
    public Style DefaultStyle => _defaultStyle ?? throw new InvalidOperationException($"StyleSet {Name} has no styles!");

    public Style this[string styleName] =>
        _styles.TryGetValue(styleName, out var style)
            ? style
            : _parent?[styleName] ?? throw new InvalidOperationException($"Style {styleName} not found in StyleSet {Name}");
    
}