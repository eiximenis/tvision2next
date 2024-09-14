using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Tvision2.Drawing.Shapes;
using Wcwidth;

namespace Tvision2.Drawing.Text;


public struct WordLineInfo
{

    public int Width; 
    public int SeparatorsBefore;
    public int SeparatorsAfter;
    
    public WordLineInfo(int width, int separatorsBefore, int separatorsAfter)
    {
        Width = width;
        SeparatorsBefore = separatorsBefore;
        SeparatorsAfter = separatorsAfter;
    }

    public int WidthWithSeparators => Width + SeparatorsBefore + SeparatorsAfter;
}

public enum Justification
{
    None,
    Left,
    Right,
    Full,
    Center
}


public enum WordSeparator
{
    None,
    Space,
    Line,
    Paragraph
}

public class Wrapper
{
    readonly record struct WordIndex(int Start, int End, WordSeparator Separator)
    {
        public int Length => End - Start + 1;
    }

    private readonly string _text;
    private readonly Rune[] _runes;
    private readonly List<WordIndex> _words = [];

    public int WordCount => _words.Count;

    public Wrapper(string text)
    {
        _text = text;
        _runes = text.EnumerateRunes().ToArray();
    }

    public WrapInfo Wrap<TShape>(string text, TShape shape, 
        Justification justification = Justification.None) 
        where TShape : IShape
    {
        SeparateWords();
        var linesLen = shape.GetLinesLengths().ToArray();                // Space per each line. Note that an Rune can occupy 0,1 or 2 spaces
        var lines = shape.HeightInside;
        var wrapInfo = new WrapInfo();
        var currentWordIdx = 0;
        var numWords = WordCount;

        for (var lineIdx = 0; lineIdx < lines; lineIdx++)
        {
            var remainingWidth = linesLen[lineIdx];
            wrapInfo.NewLine(remainingWidth);
            var currentWord = GetWordRunes(currentWordIdx);
            var currentWordWidth = Length.WidthFromRunes(currentWord);
            if (currentWordWidth >= remainingWidth)
            {
                // First word is too large for line, we simply "cut it"
                var wli = new WordLineInfo(remainingWidth, 0, 0);
                wrapInfo.AddWord(wli);
                currentWordIdx++;
                continue;
            }

            while (remainingWidth >= currentWordWidth)
            {
                var wli = remainingWidth > currentWordWidth
                    ? new WordLineInfo(currentWordWidth, 0, 1)
                    : new WordLineInfo(currentWordWidth, 0, 0);
                wrapInfo.AddWord(wli);
                currentWordIdx++;
                remainingWidth -= wli.WidthWithSeparators;    

                if (currentWordIdx == numWords)
                {
                    return wrapInfo.Justify(justification);
                }

                currentWord = GetWordRunes(currentWordIdx);
                currentWordWidth = Length.WidthFromRunes(currentWord);
            }
        }

        return wrapInfo.Justify(justification);
    }

    private void SeparateWords()
    {
        var lastWordIdx = 0;
        for (var idx = 0; idx < _runes.Length; idx++)
        {
            var rune = _runes[idx];
            var runeCategory = Rune.GetUnicodeCategory(rune);
            var (isSeparator, separator) = runeCategory switch
            {
                UnicodeCategory.ParagraphSeparator => (true, WordSeparator.Paragraph),
                UnicodeCategory.LineSeparator => (true, WordSeparator.Line),
                UnicodeCategory.SpaceSeparator => (true, WordSeparator.Space),
                _ => (false, WordSeparator.None)
            };
            if (isSeparator && idx > 0)
            {
                _words.Add(new WordIndex(lastWordIdx, idx - 1, separator));  // Separator Rune not included (use WordSeparator instead)
                lastWordIdx = idx + 1;
            }
        }

        if (lastWordIdx < _runes.Length)
        {
            _words.Add(new WordIndex(lastWordIdx, _runes.Length - 1, WordSeparator.None));
        }
    }

    public ReadOnlySpan<Rune> GetWordRunes(int idx)
    {
        var word = _words[idx];
        return _runes.AsSpan(word.Start, word.Length);
    }
}
