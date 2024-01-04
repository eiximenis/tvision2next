namespace Tvision2.Console;

public static class AnsiEscapeSequences
{
    public const string DECTCEM_VISIBLE = "\x1b[25h";
    public const string DECTCEM_HIDDEN = "\x1b[25l";
    public const string INITC = "\x1b]4;{0};rgb:{1:x}/{2:x}/{3:x}\x1b\\";
    public const string SGR_88_FORE = "\x1b[38;5;{0}m";
    public const string SGR_88_BACK = "\x1b[48;5;{0}m";
    public const string SGR_RGB_FORE = "\x1b[38;2;{0};{1};{2}m";
    public const string SGR_RGB_BACK = "\x1b[48;2;{0};{1};{2}m";
    public const string SGR_ANSI = "\x1b[{0}m";
    public const string SMCUP = "\x1b[?1049h";          // Enter alternate screen
    public const string RMCUP = "\x1b[?1049l";          // Exit alternate screen
    public const string CLEAR = "\x1b[2J";
    public const string CUP = "\x1b[{0};{1}H";
}