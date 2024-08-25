using System.Runtime.InteropServices;
using Tvision2.Console.Events;
using Tvision2.Console.Input;
using Tvision2.Console.Linux;
using Tvision2.Console.Windows;

namespace Tvision2.Console;

public interface IConsoleEventsReader
{
    void ReadEvents(TvConsoleEvents events);

    static IConsoleEventsReader GetByOs(ConsoleOptions options)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (options.Windows.UseSequences)
            {
                var seqReader = new EscapeSequenceReader();
                return new WindowsConsoleSequenceEventsReader(seqReader, new XtermSequences());
            }
            else
            {
                return new WindowsConsoleEventsReader();
            }
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var seqReader = new EscapeSequenceReader();
            return new LinuxConsoleEventsReader(seqReader, new XtermSequences());
        }

        throw new NotSupportedException("Only Windows and Linux are supported right now");
    }

    void Init();
}