using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;

namespace Tvision2.Console;
public class TvConsoleColorSet
{
    private readonly Dictionary<Type, TvColor> _fgColors = new();
    private readonly Dictionary<Type, TvColor> _bgColors = new();

    private static readonly TvConsoleColorSet _instance = new TvConsoleColorSet();
    public static TvConsoleColorSet Default => _instance;

    public  void SetForegroundColor<T>(TvColor color)
    {
        if (_fgColors.ContainsKey(typeof(T)))
        {
            _fgColors[typeof(T)] = color;
        }
        else
        {
            _fgColors.TryAdd(typeof(T), color);
        }
    }
    public void SetBackgroundColor<T>(TvColor color)
    {
        if (_bgColors.ContainsKey(typeof(T)))
        {
            _bgColors[typeof(T)] = color;
        }
        else
        {
            _bgColors.TryAdd(typeof(T), color);
        }
    }

    public void RemoveForegroundColor<T>(TvColor color) => _fgColors.Remove(typeof(T));
    public void RemoveBackgroundColor<T>(TvColor color) => _bgColors.Remove(typeof(T));

    public void Reset()
    {
        _fgColors.Clear();
        _bgColors.Clear();
    }

}
