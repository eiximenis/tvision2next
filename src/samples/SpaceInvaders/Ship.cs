using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;

namespace SpaceInvaders;
class Ship
{
    private const string SPRITE = "^";

    private TvPoint _position = TvPoint.FromXY(20, 29);

    private readonly Lazy<TvComponent<Ship>> _component;

    private readonly List<Shot> _shots = [];

    public TvPoint Position => _position;
    public TvComponent<Ship> Component => _component.Value;
    public IEnumerable<Shot> Shots => _shots;

    public Ship()
    {
        _component = new Lazy<TvComponent<Ship>>(CreateComponent);
    }

    private TvComponent<Ship> CreateComponent()
    {
        var cmp = new TvComponent<Ship>(this, new Viewport(_position, TvBounds.SingleCharacter));

        cmp.AddDrawer(ctx => ctx.DrawStringAt(SPRITE, TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.LightYellow, TvColor.Black)));
        cmp.AddBehavior(ShipBehavior);
        return cmp;
    }

    private static void ShipBehavior(BehaviorContext<Ship> ctx)
    {
        var events = ctx.Events;
        var key = events.AcquireFirstKeyboard(autoHandle: true);
        if (key is null) return;

        var ship = ctx.State;

        var ckey = key.AsConsoleKeyInfo();

        switch (ckey.Key)
        {
            case ConsoleKey.LeftArrow:
                ship._position = ship._position with { X = ship._position.X - 1 };
                ctx.Move(ship._position);
                break;
            case ConsoleKey.RightArrow:
                ship._position = ship._position with { X = ship._position.X + 1 };
                ctx.Move(ship._position);
                break;
            case ConsoleKey.Spacebar:
                var shot = new Shot(ship._position with { Y = ship._position.Y - 1 });
                ctx.ComponentTree.Add(shot.Component);
                shot.Component.Metadata.On().ComponentRemoved.Do(_ => ship._shots.Remove(shot));
                ship._shots.Add(shot);
                break;
        }
    }
}
