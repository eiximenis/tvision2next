using System.Diagnostics;
using Tvision2.Console.Events;
using Tvision2.Console.Input;
using Tvision2.Console.Linux.Interop;

namespace Tvision2.Console.Linux
{
    public class LinuxConsoleEventsReader : IConsoleEventsReader
    {
        private readonly ISequenceReader _sequenceReader;
        private const int ESC = 27;

        public LinuxConsoleEventsReader(ISequenceReader sequenceReader, IInputSequences inputSequences)
        {
            _sequenceReader = sequenceReader;
            _sequenceReader.AddSequences(inputSequences.GetSequences());
        }
        public void ReadEvents(TvConsoleEvents events)
        {
            var data = Libc.read();
            if (data == -1)
            {
                return;
            }

            Debug.WriteLine($"RE --> {data} '{(char)data}'");

            if (data == ESC)
            {
                var sequenceStarted = false;
                _sequenceReader.Start();
                var nextkey = Libc.read();
                while (nextkey != -1)
                {
                    Debug.WriteLine(nextkey + "" + (char)nextkey);
                    _sequenceReader.Push(nextkey);
                    sequenceStarted = true;
                    nextkey = Libc.read();
                }

                if (sequenceStarted)
                {
                    var sequences = _sequenceReader.CheckSequences();
                    foreach (var seq in sequences)
                    {
                        if (seq != null) events.Add(new AnsiConsoleKeyboardEvent(seq.KeyInfo));
                        else Debug.WriteLine("Unknown ESC sequence");
                    }
                }
                else
                {
                    events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, ConsoleKey.Escape, false, false,
                        false)));
                }
            }
            else
            {
                Debug.WriteLine("Data " + data);
                switch (data)
                {
                    case 9:
                        events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo('\t', ConsoleKey.Tab, false, false,
                            false)));
                        break;
                    case 13:
                        events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false,
                            false)));
                        break;
                    case 127:
                        events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, ConsoleKey.Backspace, false,
                            false, false)));
                        break;
                    default:
                        if (data < 26) // 1 is ^A ... 26 is ^Z. Note that 9 is also ^I and ^M is 13 both already handled before
                        {
                            events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)(data - 1 + 'A'),
                                ConsoleKey.A + data - 1, false, false, true)));
                        }
                        else
                        {
                            events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, (ConsoleKey)data, false,
                                false, false)));
                        }

                        break;
                }
            }
        }
    }
}