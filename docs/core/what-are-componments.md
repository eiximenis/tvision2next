# Tvision2 Components

A Tvision2 application is composed of _components_. A component is a instance of `TvComponent<TState>` class and has four important parts:

1. The **state** or the data that the component owns
2. The **behavior** or what the component does
3. The **drawer** or how the component renders on console
4. The **viewport** or where the component is rendered

## The component State

All components have one state which is the data owned by this component. The State can be as simple as a `int` or `string` or any class you want to use. To create a component can use the method `TvComponent.Create<TState>` that returns an instance of `TvComponent<TState>` class with specified state:

```cs
var cmp = TvComponent.Create("Hello World");    // Returns a TvComponent<string> instance
```

> The class TvComponent<TState> is sealed so it can't be inherited. The way you create custom component objects is using any class that holds a `TvComponent<TState>` as a member.

Every time the state of the component changes, the component will trigger a render (there are other actions that can trigger a render, but changing state is the most important one). The state of the component can be changed by calling the `SetState` method with the new state or inside a behavior.

## Component Behaviors

A component can have zero or more behaviors attached to it. All behaviors of a component have a chance to run during the _Update_ cycle. A behavior can:

- Change State of its own component
- Change Viewport of its own component

Following code adds a behavior to a component that adds 1 to its state:

```cs
cmpint.AddBehavior(ctx => ctx.ReplaceState(ctx.State + 1));
```

## Component Drawers

A component can have zero or more drawers attached to it. All drawers of the component are invoked to render the component in the _Draw_ cycle.

```cs
cmpstr.AddDrawer((ctx, state) => ctx.DrawStringAt(state, TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.Blue, TvColor.LightBlack)));
```


