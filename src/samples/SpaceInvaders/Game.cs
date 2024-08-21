using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Components;

namespace SpaceInvaders;
class Game : IHook
{
    private readonly TvUiManager _ui;
    private readonly Ship _ship;
    private readonly List<Invader> _invaders = new List<Invader>();

    private readonly TvComponent<int> _counter;
    public Game(Tvision2Engine engine)
    { 
        _ui = engine.UI;
        _ship = new Ship();
        _counter = new TvComponent<int>(0, Viewports.Null());
        _counter.AddDrawer((ctx, c) => ctx.DrawStringAt($"Remaining: {c}", TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.FromHexString("#666600"), TvColor.Black)));
        _ui.ComponentTree.Add(_counter);
    }

    public Task Init()
    {
        for (var idx = 0; idx < 10; idx++)
        {
            var invader = new Invader();
            _invaders.Add(invader);
            _ui.ComponentTree.Add(invader.Component);
        }
        _ui.ComponentTree.Add(_ship.Component);
        _counter.SetState(_invaders.Count);
        _counter.Viewport.MoveAndResize(TvPoint.FromXY(40, 0), TvBounds.FromRowsAndCols(1, 15));

        return Task.CompletedTask;
    }

    public Task BeforeUpdate(TvConsoleEvents events)
    {
        var invadersToKill = new List<Invader>();
        foreach (var shot in _ship.Shots)
        {
            foreach (var invader in _invaders)
            {
                if (invader.Position == shot.Position)
                {
                    _ui.ComponentTree.Remove(invader.Component);
                    invadersToKill.Add(invader);
                }
            }
        }

        if (invadersToKill.Count > 0)
        {
            foreach (var invader in invadersToKill)
            {
                _invaders.Remove(invader);
            }

            _counter.SetState(_invaders.Count);
        }

        return Task.CompletedTask;
    }
}
