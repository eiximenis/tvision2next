using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Shapes;
using Wcwidth;

namespace Tvision2.Drawing.Text;
public static class WrappedText
{
    public static void Draw<TShape>(IConsoleDrawer drawer, string text, TShape shape, TvColorsPair colors, Justification justification = Justification.None)
        where TShape : IShape
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

                drawer.DrawChars(' ', wordInLine.SeparatorsBefore, TvPoint.FromXY(shape.TopLeftInside.X, py), colors);
                displacement += wordInLine.SeparatorsBefore;
                var writtenWidth = 0;
                var deltaX = 0;
                for (var runeIdx = 0; runeIdx < wordRunes.Length && writtenWidth < wordInLine.Width;)
                {
                    var px = shape.TopLeftInside.X + displacement + deltaX;
                    if (shape.PointIsInside(TvPoint.FromXY(px, py)))
                    {
                        drawer.DrawRunes(wordRunes[runeIdx], 1, TvPoint.FromXY(px, py), colors );
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

                drawer.DrawChars(' ', wordInLine.SeparatorsAfter, TvPoint.FromXY(shape.TopLeftInside.X + displacement + deltaX , py), colors);
                displacement += wordInLine.SeparatorsAfter;
            }
        }
    }
}
