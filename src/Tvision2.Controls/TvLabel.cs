namespace Tvision2.Controls;

public class TvLabel : TvControl<string, string>
{
    public TvLabel(string text, string? name) : base(name ?? "label", text )
    {
    }   
    
}