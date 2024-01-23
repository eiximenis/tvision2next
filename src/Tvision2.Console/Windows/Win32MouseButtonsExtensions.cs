using Tvision2.Console.Events;

namespace Tvision2.Console.Windows;

static class Win32MouseButtonsExtensions
{
    public static TvMouseButtonStates ToReleasedButtonStates(this Win32MouseButtons buttons)
    {
        var states = TvMouseButtonStates.None;
        if ((buttons & Win32MouseButtons.FromLeft1st) == Win32MouseButtons.FromLeft1st)
            states |= TvMouseButtonStates.LeftButtonReleased;
        if ((buttons & Win32MouseButtons.FromLeft2nd) == Win32MouseButtons.FromLeft2nd)
            states |= TvMouseButtonStates.SecondLeftButtonReleased;
        if ((buttons & Win32MouseButtons.FromLeft3rd) == Win32MouseButtons.FromLeft3rd)
            states |= TvMouseButtonStates.ThirdLeftButtonReleased;
        if ((buttons & Win32MouseButtons.Right) == Win32MouseButtons.Right)
            states |= TvMouseButtonStates.RightButtonReleased;
        return states;
    }

    public static TvMouseButtonStates ToPressedButtonStates(this Win32MouseButtons buttons)
    {
        var states = TvMouseButtonStates.None;
        if ((buttons & Win32MouseButtons.FromLeft1st) == Win32MouseButtons.FromLeft1st)
            states |= TvMouseButtonStates.LeftButtonPressed;
        if ((buttons & Win32MouseButtons.FromLeft2nd) == Win32MouseButtons.FromLeft2nd)
            states |= TvMouseButtonStates.SecondLeftButtonPressed;
        if ((buttons & Win32MouseButtons.FromLeft3rd) == Win32MouseButtons.FromLeft3rd)
            states |= TvMouseButtonStates.ThirdLeftButtonPressed;
        if ((buttons & Win32MouseButtons.Right) == Win32MouseButtons.Right)
            states |= TvMouseButtonStates.RightButtonPressed;
        return states;
    }

    public static TvMouseButtonStates ToDoubleClickButtonStates(this Win32MouseButtons buttons)
    {
        var states = TvMouseButtonStates.None;
        if ((buttons & Win32MouseButtons.FromLeft1st) == Win32MouseButtons.FromLeft1st)
            states |= TvMouseButtonStates.LeftButtonDoubleClicked;
        if ((buttons & Win32MouseButtons.FromLeft2nd) == Win32MouseButtons.FromLeft2nd)
            states |= TvMouseButtonStates.SecondLeftButtonDoubleClicked;
        if ((buttons & Win32MouseButtons.FromLeft3rd) == Win32MouseButtons.FromLeft3rd)
            states |= TvMouseButtonStates.ThirdLeftButtonDoubleClicked;
        if ((buttons & Win32MouseButtons.Right) == Win32MouseButtons.Right)
            states |= TvMouseButtonStates.RightButtonDoubleClicked;
        return states;
    }
}