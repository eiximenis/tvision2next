namespace Tvision2.Controls;

public class TvControlMetadata
{

    public ITvControl Control { get; }

    public TvControlMetadata(ITvControl owner)
    {
        Control = owner; 
    }
    
}