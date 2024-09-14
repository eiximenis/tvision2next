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
using Wcwidth;

namespace Tvision2.Console;

partial class TvConsole
{
    public static void Wrap<TShape>(string text, TShape shape, Justification justification = Justification.None) where TShape : IShape
    {
        var wrapper = new Wrapper(text);
        var wrapInfo = wrapper.Wrap(text, shape, justification);
        var wordIndex = 0;
        for (var idx = 0; idx < wrapInfo.UsedLines; idx++)
        {
            var wordsInLine = wrapInfo.GetWordsForLine(idx);
            var displacement = 0;
            foreach (var wordInLine in wordsInLine)
            {
                var wordRunes = wrapper.GetWordRunes(wordIndex++);
                var py = shape.TopLeftInside.Y + idx;

                MoveCursorTo(shape.TopLeftInside.X, py);
                for (var before = 0; before < wordInLine.SeparatorsBefore; before++)
                {
                    Write(' ');
                }

                displacement += wordInLine.SeparatorsBefore;
                var writtenWidth = 0;
                var deltaX = 0;
                for (var runeIdx = 0; runeIdx < wordRunes.Length && writtenWidth < wordInLine.Width;)
                {
                    var px = shape.TopLeftInside.X + displacement + deltaX;
                    if (shape.PointIsInside(TvPoint.FromXY(px, py)))
                    {
                        MoveCursorTo(px, py);
                        Write(wordRunes[runeIdx]);
                        var runeWidth = UnicodeCalculator.GetWidth(wordRunes[runeIdx]);
                        writtenWidth += runeWidth;
                        displacement += runeWidth;
                        runeIdx++;
                    }
                    else
                    {
                        deltaX++;
                    }
                }
                for (var after = 0; after < wordInLine.SeparatorsAfter; after++)
                {
                    Write(' ');
                }

                displacement += wordInLine.SeparatorsAfter;
            }
        }

    }
};