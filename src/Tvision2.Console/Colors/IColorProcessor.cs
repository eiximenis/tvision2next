using Tvision2.Core;
using Tvision2.Core.Console;

namespace Tvision2.Console.Colors;

public interface IColorProcessor
{
    public TvColor Process(TvColor original, CharacterAttributeModifiers attributtes);
}