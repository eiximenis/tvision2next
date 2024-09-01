using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Console.Events;

namespace Tvision2.Engine.Input;
public class InputHandler
{
    public StringBuilder _currentString = new StringBuilder();

    public string GetString() => _currentString.ToString();


    public void Reset()
    {
        _currentString.Clear();
    }

    public void ProcessInputEvents(TvConsoleEvents events)
    {

    }
}
