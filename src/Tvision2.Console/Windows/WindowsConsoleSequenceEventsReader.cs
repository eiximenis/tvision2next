using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tvision2.Console.Events;
using Tvision2.Console.Input;
using Tvision2.Console.Windows.Interop;
using static Tvision2.Console.Windows.Interop.Types;

namespace Tvision2.Console.Windows;
public class WindowsConsoleSequenceEventsReader : IConsoleEventsReader
{
    private readonly ISequenceReader _sequenceReader;
    private const int ESC = 27;
    private readonly IntPtr _hstdin;
    private uint _initialConsoleModes = 0;


    public WindowsConsoleSequenceEventsReader()
    {
        _hstdin = WindowsNative.GetStdHandle(WindowsNative.STDIN);
    }
    public WindowsConsoleSequenceEventsReader(ISequenceReader sequenceReader, IInputSequences inputSequences)
    {
        _sequenceReader = sequenceReader;
        _sequenceReader.AddSequences(inputSequences.GetSequences());
    }

    public void Init()
    {
        WindowsNative.GetConsoleMode(_hstdin, out _initialConsoleModes);

        var enableVtInput = (ConsoleInputModes.ENABLE_MOUSE_INPUT
                             | ConsoleInputModes.ENABLE_WINDOW_INPUT
                             | ConsoleInputModes.ENABLE_VIRTUAL_TERMINAL_INPUT)
                            & ~ConsoleInputModes.ENABLE_QUICK_EDIT_MODE
                            & ~ConsoleInputModes.ENABLE_ECHO_INPUT
                            & ~ConsoleInputModes.ENABLE_INSERT_MODE
                            & ~ConsoleInputModes.ENABLE_LINE_INPUT;
        WindowsNative.SetConsoleMode(_hstdin, (uint)enableVtInput);
    }


    public void ReadEvents(TvConsoleEvents events)
    {

    }

}
