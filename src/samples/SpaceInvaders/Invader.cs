using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;

namespace SpaceInvaders;
class Invader
{
    private const string SPRITE = "👾";

    private TvPoint _position;

    public TvPoint Position => _position;

    private readonly Lazy<TvComponent<Unit>> _component;
    public TvComponent<Unit> Component => _component.Value;

    public Invader()
    {
        var random = new Random();
        _position = TvPoint.FromXY(random.Next(0, 60), random.Next(0, 4));
        _component = new Lazy<TvComponent<Unit>>(CreateComponent);
    }
    private TvComponent<Unit> CreateComponent()
    {
        var cmp = TvComponent.CreateStatelessComponent(new Viewport(_position, TvBounds.SingleCharacter));

        cmp.AddDrawer(ctx => ctx.DrawStringAt(SPRITE, TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Black)));
        cmp.AddBehavior(InvaderBehavior);
        return cmp;
    }

    private void InvaderBehavior(BehaviorContext<Unit> ctx)
    {
        _position = _position with { X = _position.X - 1 };
        if (_position.X < 0) _position = TvPoint.FromXY(60, _position.Y + 1);

        ctx.Move(_position);
    }
    
}
