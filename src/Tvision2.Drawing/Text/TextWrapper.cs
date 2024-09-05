using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Drawing.Shapes;
using Wcwidth;

namespace Tvision2.Drawing.Text;


public readonly struct WrapInfo
{
    private readonly List<List<int>> _wordsPerLine;
    public WrapInfo()
    {
        _wordsPerLine = [];
    }
    internal void NewLine() => _wordsPerLine.Add([]);
    internal void AddWord(int wordIdx) => _wordsPerLine[^1].Add(wordIdx);

    public int UsedLines => _wordsPerLine.Count;
    public IEnumerable<int> GetWordsForLine(int lineIdx) => _wordsPerLine[lineIdx];
}

public class Wrapper
{
    readonly record struct WordIndex(int Start, int End)
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

    public WrapInfo Wrap<TShape>(string text, TShape shape) where TShape : IShape
    {
        SeparateWords();
        var linesLen = shape.GetLinesLengths().ToArray();                // Space per each line
        var lines = shape.HeightInside;
        var wrapInfo = new WrapInfo();
        var currentWordIdx = 0;
        var numWords = WordCount;

        for (var lineIdx = 0; lineIdx < lines; lineIdx++)
        {
            wrapInfo.NewLine();
            var remainingLen = linesLen[lineIdx];
            var currentWordLen = 0;
            while (remainingLen >= currentWordLen)
            {
                var currentWord = GetWordRunes(currentWordIdx);
                Debug.WriteLine(currentWord.ToString());
                currentWordLen = Wrapper.GetWordLen(currentWord);
                wrapInfo.AddWord(currentWordIdx);
                currentWordIdx++;
                remainingLen -= currentWordLen + 1;     // +1 for the space
                if (currentWordIdx == numWords)
                {
                    return wrapInfo;
                }
            }
        }

        return wrapInfo;
    }

    private void SeparateWords()
    {
        var lastWordIdx = 0;
        for (var idx = 0; idx < _runes.Length; idx++)
        {
            var rune = _runes[idx];
            if (Rune.IsWhiteSpace(rune) && idx > 0)
            {
                _words.Add(new WordIndex(lastWordIdx, idx));
                lastWordIdx = idx + 1;
            }
        }

        if (lastWordIdx != _runes.Length - 1)
        {
            _words.Add(new WordIndex(lastWordIdx, _runes.Length - 1));
        }
    }

    public ReadOnlySpan<Rune> GetWordRunes(int idx)
    {
        var word = _words[idx];
        return _runes.AsSpan(word.Start, word.Length);
    }
    private static int GetWordLen(ReadOnlySpan<Rune> word)
    {
        var len = 0;
        for (var idx = 0; idx < word.Length; idx++)
        {
            len += UnicodeCalculator.GetWidth(word[idx]);
        }

        return len;
    }
}
