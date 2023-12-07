namespace Tvision2.Console;

public enum EscapeSequence
{
    DECTCEM_VISIBLE = 1,            // Show Cursor
    DECTCEM_HIDDEN,                 // Hide Cursor
    INITC,                          // Change Palette
    SGR_88_FORE,                    // Change forecolor (palettzed) (SETAF)
    SGR_88_BACK,                    // Change backcolor (palettized) (SETAB)
    SGR_RGB_FORE,                   // Change forecolor (truecolor) (SETAF)
    SGR_RGB_BACK,                   // Change forecolor (trucolor) (SETAB)
    SGR_ANSI,                       // ANSI change color (SETAF / SETAB)
    SMCUP,                          // Enter TUI mode
    CLEAR                           // Clear Screen
}    
    
