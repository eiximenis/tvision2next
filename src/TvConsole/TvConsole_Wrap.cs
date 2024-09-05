using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Tvision2.Core;
using Tvision2.Drawing.Shapes;
using Tvision2.Drawing.Text;

namespace Tvision2.Console;

partial class TvConsole
{
    public static void Wrap<TShape>(string text, TShape shape) where TShape : IShape
    {
        var wrapper = new Wrapper(text);
        var wrapInfo = wrapper.Wrap(text, shape);
        for (var idx = 0; idx < wrapInfo.UsedLines; idx++)
        {
            var wordsInLine = wrapInfo.GetWordsForLine(idx);
            var displacement = 0;
            foreach (var wordIdx in wordsInLine)
            {
                var wordRunes = wrapper.GetWordRunes(wordIdx);
                var py = shape.TopLeftInside.Y + idx;
                
                for (var runeIdx = 0; runeIdx < wordRunes.Length; )
                {
                    var px = shape.TopLeftInside.X + displacement;
                    if (shape.PointIsInside(TvPoint.FromXY(px, py)))
                    {
                        MoveCursorTo(px, py);
                        Write(wordRunes[runeIdx]);
                        displacement++;
                        runeIdx++;
                    }
                }
            }
        }

    }
};