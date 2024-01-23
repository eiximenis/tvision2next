namespace Tvision2.Console.Windows;
using static Tvision2.Console.Windows.Interop.Types;

static class ControlKeyStatesExtensions
{
    public static (bool ctrl, bool alt, bool shift) GetModifiers (this ControlKeyStates dwControlKeyState)
    {
        var shift = (dwControlKeyState & ControlKeyStates.SHIFT_PRESSED) == ControlKeyStates.SHIFT_PRESSED;
        var alt = (dwControlKeyState & ControlKeyStates.LEFT_ALT_PRESSED) == ControlKeyStates.LEFT_ALT_PRESSED ||
                  (dwControlKeyState & ControlKeyStates.RIGHT_ALT_PRESSED) == ControlKeyStates.RIGHT_ALT_PRESSED;
        var control = (dwControlKeyState & ControlKeyStates.LEFT_CTRL_PRESSED) == ControlKeyStates.LEFT_CTRL_PRESSED ||
                      (dwControlKeyState & ControlKeyStates.RIGHT_CTRL_PRESSED) == ControlKeyStates.RIGHT_CTRL_PRESSED;
        return (control, alt, shift);
    }
}