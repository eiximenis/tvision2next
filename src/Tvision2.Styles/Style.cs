namespace Tvision2.Styles;

public class Style
{
    private readonly Dictionary<string, StyleState> _states;
    
    public const string DefaultStateName = "default";
    public string Name { get; }
    public IEnumerable<StyleState> States => _states.Values;
    public StyleState DefaultState { get; private set; }

    
    public StyleState this[string stateName] => _states.TryGetValue(stateName, out var state) 
        ? state 
        : throw new InvalidOperationException($"Style {Name} does not contain state {stateName}" );

    public StyleState GetStyleOrDefault(string stateName) => _states.TryGetValue(stateName, out var state)
        ? state
        : DefaultState;
    
    public Style(string name)
    {
        Name = name;
        _states = new Dictionary<string, StyleState>();
    }
    internal void Add(StyleState state)
    {
        _states.Add(state.Name, state);
        if (state.Name == DefaultStateName || _states.Count == 1)
        {
            DefaultState = state;
        }
    }

    internal void Merge(Style otherStyle)
    {
        foreach (var otherState in otherStyle.States)
        {
            var key = otherState.Name;
            if (_states.ContainsKey(key))
            {
                throw new InvalidOperationException(
                    $"State {key} found in both current and to be merged styles (for style {Name}). States can't be merged!");
            }
            Add(otherState);
        }
    }
}