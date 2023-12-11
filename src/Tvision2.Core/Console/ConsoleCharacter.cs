using System.Text;

namespace Tvision2.Core.Console;

public readonly record struct ConsoleCharacter(Rune Character, CharacterAttribute Attributes)
{
    public ConsoleCharacter(char character, CharacterAttribute attrs) : this (new Rune (character), attrs)
    {
    }

    public static ConsoleCharacter Null  {  get => new ConsoleCharacter(); }
}