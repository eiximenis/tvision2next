using Tvision2.Core.Console;

namespace Tvision2.Core.Engine.Render;

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
