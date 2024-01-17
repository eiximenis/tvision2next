namespace Tvision2.Engine.Components;

public readonly struct LayerSelector
{

    private const byte NONE_INDEX = 0;
    private const byte TOP_INDEX= byte.MaxValue;
    private const byte STANDARD_INDEX = 127;
    private const byte BOTTOM_INDEX = 2;
    private const byte BG_INDEX = 1;
    
    public static int MaxLayerIndex => byte.MaxValue;
    public static LayerSelector Top => new(TOP_INDEX);
    public static LayerSelector Bottom => new(BOTTOM_INDEX);
    public static LayerSelector Standard => new(STANDARD_INDEX);
    public static LayerSelector None => new (NONE_INDEX);

    internal static LayerSelector Background => new(BG_INDEX);

    public static LayerSelector FromIndex(byte index)
    {
        if (index == 0) throw new ArgumentException("Layer index starts at one");
        return new LayerSelector(index);
    }
    public byte LayerIndex { get; }
    
    public bool IsNone => LayerIndex == NONE_INDEX;
    
    private LayerSelector(byte layerIndex)
    {
        LayerIndex = layerIndex;
    }

    public static int CompareBottomFirst(LayerSelector l1, LayerSelector l2) => l1.LayerIndex - l2.LayerIndex;
    public static int CompareTopFirst(LayerSelector l1, LayerSelector l2) => l2.LayerIndex - l1.LayerIndex;

    public int CompareWith(LayerSelector layer)
    {
        return LayerIndex - layer.LayerIndex;
    }
    
}