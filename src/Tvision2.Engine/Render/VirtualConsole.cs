using Tvision2.Core;
using Tvision2.Core.Console;
using Tvision2.Engine.Components;
using Wcwidth;

namespace Tvision2.Engine.Render;

public class VirtualConsole
{   enum DirtyStatus
    {
        Clean = 0,
        Dirty = 1
    }
    
    private readonly ConsoleCharacter[] _buffer;
    private readonly DirtyStatus[] _dirtyMap;
    private readonly TvColor _defaultBg;
    
    public TvBounds Bounds { get; }
    public bool IsDirty { get; private set; }
    
   public void DrawAt(string text,  TvPoint location, CharacterAttribute attr, in Viewzone cropzone)
   {
       if (!cropzone.ContainsLine(location.Y) || !cropzone.ContainsColumn(location.X))
       {
           return;
       }
       
       var lineBuffer = _buffer.AsSpan(location.Y * Bounds.Width, Bounds.Width);
       var dirtyBuffer = _dirtyMap.AsSpan(location.Y * Bounds.Width, Bounds.Width);
       var startcol = location.X;
       var endcol = Math.Min(cropzone.BottomRight.X, Bounds.Width -1);  
       var dirty = IsDirty;
       var runes = text.EnumerateRunes();
       var idx = startcol;

       while (runes.MoveNext())
       {
           var rune = runes.Current;
           ref var cchar = ref lineBuffer[idx];
           var newchar = new ConsoleCharacter(rune, attr);
           var runeWidth = UnicodeCalculator.GetWidth(rune);
           if (runeWidth > 0)
           {
               if (!cchar.Equals(newchar))
               {
                   lineBuffer[idx] = newchar;
                   dirtyBuffer[idx] = DirtyStatus.Dirty;
                   dirty = true;

                   if (runeWidth == 2)
                   {
                       lineBuffer[idx + 1] = ConsoleCharacter.Null;
                   }
               }
               idx += runeWidth;
               if (idx > endcol) { break; }
           }
       }

       IsDirty = dirty;
   }
   
   public void FillRect(TvPoint location, CharacterAttribute attr,in Viewzone cropzone)
   {
       if (!cropzone.ContainsLine(location.Y) || !cropzone.ContainsColumn(location.X))
       {
           return;
       }

       var dirty = false;
       
       var startcol = location.X;
       var endcol = Math.Min(cropzone.BottomRight.X, Bounds.Width -1);
       var startrow = location.Y;
       var endrow = Math.Min(cropzone.BottomRight.Y, Bounds.Height - 1);


       for (var crow = startrow; crow <= endrow; crow++)
       {
           var lineBuffer = _buffer.AsSpan(crow * Bounds.Width, Bounds.Width);
           var dirtyBuffer = _dirtyMap.AsSpan(crow * Bounds.Width, Bounds.Width);
           for (var ccol = startcol; ccol <= endcol; ccol++)
           {
               var newchar = new ConsoleCharacter(' ', attr);
               ref var cchar = ref lineBuffer[ccol];
               
               if (cchar != newchar)
               {
                   lineBuffer[ccol] = newchar;
                   dirtyBuffer[ccol] = DirtyStatus.Dirty;
                   dirty = true;
               }
           }
       }
       IsDirty = dirty;
   }
    
    public VirtualConsole(TvBounds bounds, TvColor defaultBack)
    {
        Bounds = bounds;
        _defaultBg = defaultBack;
        _buffer = new ConsoleCharacter[bounds.Length];
        _dirtyMap = new DirtyStatus[bounds.Length];
        IsDirty = true;
    }
    
    public void Flush(IConsoleDriver consoleDriver)
    {
        if (!IsDirty) { return; }
        
        var height = Bounds.Height;
        var width = Bounds.Width;
        var idx = 0;
        for (var row = 0; row < height; row++)
        {
            var span = 1;
            var spanning = false;
            var spanCol = 0;
            for (var col = 0; col < width; col++)
            {

                ref var cc = ref _buffer[idx];
                if (spanning || _dirtyMap[idx] != DirtyStatus.Clean)
                {
                    var compareWithNext = col < width - 1;
                    if (compareWithNext && cc.Equals(_buffer[idx + 1]))
                    {
                        span++;
                        spanning = true;
                    }
                    else
                    {
                        consoleDriver.WriteCharactersAt(spanCol, row, span, cc.Character, cc.Attributes);
                        span = 1;
                        spanning = false;
                        spanCol = col + 1;
                    }
                }
                else
                {
                    spanCol++;
                }
                _dirtyMap[idx] = DirtyStatus.Clean;
                idx++;
            }
        }
        IsDirty = false;
    }
}
