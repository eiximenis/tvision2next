using System.Runtime.InteropServices;

namespace Tvision2.Console.Windows.Interop;

[Flags]
internal enum ConsoleInputModes : uint
{
    ENABLE_ECHO_INPUT = 0x0004,
    ENABLE_INSERT_MODE = 0x0020,
    ENABLE_LINE_INPUT = 0x0002,
    ENABLE_MOUSE_INPUT = 0x0010,
    ENABLE_PROCESSED_INPUT = 0x0001,
    ENABLE_QUICK_EDIT_MODE = 0x0040,
    ENABLE_WINDOW_INPUT = 0x0008,
    ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200,
    ENABLE_EXTENDED_FLAGS = 0x0080
}

internal enum ConsoleOutputModes : uint
{
    ENABLE_PROCESSED_OUTPUT = 0x001,
    ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
    ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
    DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
    ENABLE_LVB_GRID_WORLDWIDE = 0x0010
}

internal class WindowsNative
{
    public const int STDIN = -10;
    public const int STDOUT = -11;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetConsoleMode(IntPtr hConsoleHandle, [Out] out uint dwMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetNumberOfConsoleInputEvents(IntPtr hConsoleInput, out uint lpcNumberOfEvents);
    [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode)]
    public static extern bool ReadConsoleInput(IntPtr hConsoleInput, ref byte lpBuffer, uint nLength, out uint lpNumberOfEventsRead);
}