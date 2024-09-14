using System.Runtime.InteropServices;

namespace Tvision2.Drawing.Text;

public readonly struct WrapInfo
{
    private readonly List<List<WordLineInfo>> _wordsPerLine;
    private readonly List<int> _lineWidths;
    public WrapInfo()
    {
        _wordsPerLine = [];
        _lineWidths = [];
    }

    internal void NewLine(int lineWidth)
    {
        _lineWidths.Add(lineWidth);
        _wordsPerLine.Add([]);
    }

    internal void AddWord(WordLineInfo wordLineInfo) => _wordsPerLine[^1].Add(wordLineInfo);
    public int UsedLines => _wordsPerLine.Count;
    public IEnumerable<WordLineInfo> GetWordsForLine(int lineIdx) => _wordsPerLine[lineIdx];
    public WrapInfo Clone()
    {
        var newWrapInfo = new WrapInfo();
        newWrapInfo._lineWidths.AddRange(_lineWidths);
        for (var idx = 0; idx < _wordsPerLine.Count; idx++)
        {
            newWrapInfo._wordsPerLine.Add([.._wordsPerLine[idx]]);
        }
        return newWrapInfo;
    }
    public WrapInfo Justify(Justification justification)
    {
        if (justification == Justification.None)
        {
            return this;
        }

        for (var lineIdx = 0; lineIdx < _lineWidths.Count; lineIdx++)
        {
            Span<WordLineInfo> words = CollectionsMarshal.AsSpan(_wordsPerLine[lineIdx]);
            var lineWidth = _lineWidths[lineIdx];
            var totalWidth = 0;
            for (var idx = 0; idx < words.Length; idx++)
            {
                words[idx].SeparatorsAfter = idx < words.Length - 1 ? 1 : 0;
                totalWidth += words[idx].WidthWithSeparators;
            }
            var extraSpaces = lineWidth - totalWidth; ;
            switch (justification)
            {
                case Justification.Left:
                    words[^1].SeparatorsAfter += extraSpaces;
                    break;
                case Justification.Right:
                    words[0].SeparatorsBefore = extraSpaces;
                    break;
                case Justification.Full:
                    var numWords = words.Length;
                    if (numWords > 1)
                    {
                        var spacesPerWord = extraSpaces / (numWords - 1);
                        var remainingSpaces = extraSpaces % (numWords - 1);
                        for (var idx = 0; idx < numWords - 1; idx++)
                        {
                            words[idx].SeparatorsAfter += spacesPerWord;
                            if (remainingSpaces > 0)
                            {
                                words[idx].SeparatorsAfter++;
                                remainingSpaces--;
                            }
                        }
                    }
                    else
                    {
                        words[0].SeparatorsAfter = extraSpaces;
                    }
                    break;
                case Justification.Center:
                    var leftSpaces = extraSpaces / 2;
                    var rightSpaces = extraSpaces - leftSpaces;
                    words[0].SeparatorsBefore = leftSpaces;
                    words[^1].SeparatorsAfter = rightSpaces;
                    break;
            }
        }
        return this;
    }
}