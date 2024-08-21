using System.Text;

namespace Tvision2.Core.Console;

public interface IConsoleDriver
{
    void WriteCharacterAt(int x, int y, Rune character, CharacterAttribute attribute);
    void WriteCharactersAt(int x, int y, int count, Rune character, CharacterAttribute attribute);
    void SetCursorAt(int x, int y);
    void Init() { }
    void Teardown()  { }
    void SetCursorVisibility(bool isVisible);
    void ClearScreen(TvColor fore, TvColor back);
    void SetWindowsSize(TvBounds value);
    bool CanChangeWindowsSize { get; }
}