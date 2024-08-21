using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;

namespace SpaceInvaders;
class Shot
{
    private const string SPRITE = "|";
    private readonly Lazy<TvComponent<Unit>> _component;
    public TvComponent<Unit> Component => _component.Value;
    private TvPoint _position;

    public TvPoint Position => _position;

    public Shot(TvPoint position)
    {
        _position = position;
        _component = new Lazy<TvComponent<Unit>>(CreateComponent);
    }

    private TvComponent<Unit> CreateComponent()
    {
        var cmp = TvComponent.CreateStatelessComponent(new Viewport(_position, TvBounds.SingleCharacter));

        cmp.AddDrawer(ctx => ctx.DrawStringAt(SPRITE, TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.Red, TvColor.Black)));
        cmp.AddBehavior(ShotBehavior);
        return cmp;
    }

    private void ShotBehavior(BehaviorContext<Unit> ctx)
    {
        _position = _position with { Y = _position.Y - 1 };
        if (_position.Y == 0)
        {
            ctx.ComponentTree.Remove(_component.Value);
        }
        else
        {
            ctx.Move(_position);
        }
    }
}
