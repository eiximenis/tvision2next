namespace Tvision2.Console.Windows;

enum Win32MouseEventFlags : uint
{
    ClickOrUp = 0x0,
    MouseMoved = 0x1,
    DoubleClick = 0x2,
    Mouse_Wheeled = 0x4,
    Mouse_HorWheeled = 0x8
}