namespace Tvision2.Console.Windows;

[Flags]
enum Win32MouseButtons : uint
{
    None = 0x0,
    FromLeft1st = 0x1,
    Right = 0x2,
    FromLeft2nd = 0x4,
    FromLeft3rd= 0x8,
    FromLeft4th = 0x10
}