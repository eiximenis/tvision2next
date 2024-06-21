using System.Security.Cryptography.X509Certificates;

namespace Tvision2.Styles;

public class Style
{
    private readonly Dictionary<string, StyleState> _states;
    
    public const string DefaultStateName = "";
    public string Name { get; }
    public IEnumerable<StyleState> States => _states.Values;

    private StyleState? _defultState;
    public StyleState DefaultState => _defultState ?? throw new InvalidOperationException($"Style {Name} is empty!!!!");

    private Style? _parent;


    public StyleState this[string stateName] => _states.TryGetValue(stateName, out var state) 
        ? state 
        : throw new InvalidOperationException($"Style {Name} does not contain state {stateName}" );

    public StyleState GetDirectStateOrDefault(string stateName) => _states.TryGetValue(stateName, out var state)
        ? state
        : DefaultState;

    public StyleState GetDirectStateOrPassed(string stateName, StyleState stateToReturnIfNotFound) => _states.TryGetValue(stateName, out var state)
        ? state
        : stateToReturnIfNotFound;

    public StyleState GetStateOrDefault(string stateName) => _states.TryGetValue(stateName, out var state)
        ? state
        : _parent?.GetStateOrPassed(stateName, DefaultState) ?? DefaultState;
    
    public StyleState GetStateOrPassed(string stateName, StyleState stateToReturnIfNotFound) => _states.TryGetValue(stateName, out var state)
        ? state
        : _parent?.GetStateOrPassed(stateName, stateToReturnIfNotFound) ?? stateToReturnIfNotFound;

    public Style(string name)
    {
        Name = name;
        _states = new Dictionary<string, StyleState>();
    }
    internal void SetParent(Style parent)
    {
        _parent = parent;
    }

    internal void Add(StyleState state)
    {
        _states.Add(state.Name, state);
        if (state.Name == DefaultStateName)
        {
            _defultState = state;
        }
    }
}