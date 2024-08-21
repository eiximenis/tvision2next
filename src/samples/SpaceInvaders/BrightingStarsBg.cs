using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace SpaceInvaders;


class FallingStar
{
    public TvPoint Position { get; private set; }

    private byte _initialR;
    private byte _initialG;
    private byte _initialB;

    private const byte FinalBright = 0xFF;

    private TvColor _color;

    public TvColor Color => _color;

    public FallingStar()
    {
        var random = new Random();
        _initialR = (byte)random.Next(0x1, 0xCC);
        _initialG = (byte)random.Next(0x1, 0xCC);
        _initialB = (byte)random.Next(0x1, 0xCC);
        _color = TvColor.FromRgb(_initialR, _initialG, _initialB);
        
        Position = TvPoint.FromXY(random.Next(80), random.Next(25));
    }

    public void ChangeLighting()
    {
        var (r, g, b) = _color.Rgb;

        r = r == FinalBright ? _initialR : (byte)(r + 1);
        g = g == FinalBright ? _initialG : (byte)(g + 1);
        b = b == FinalBright ? _initialB : (byte)(b + 1);
        _color = TvColor.FromRgb(r, g, b);
    }
}


class BrightingStarsBg
{
    private const int NUM_STARS = 25;

    private FallingStar[] _stars = new FallingStar[NUM_STARS];

    public BrightingStarsBg()
    {
        for (var idx = 0; idx < NUM_STARS; idx++)
        {
            _stars[idx] = new FallingStar();
        }
    }

    internal void Draw(ConsoleContext context)
    {
        context.Fill(TvColor.Black);
        foreach (var star in _stars)
        {
            context.DrawStringAt(".", star.Position,
                TvColorsPair.FromForegroundAndBackground(star.Color, TvColor.Black));
        }
    }

    internal void Update(BehaviorContext<Unit> context)
    {
        foreach (var star in _stars)
        {
            star.ChangeLighting();
        }
    }

}
