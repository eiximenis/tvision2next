namespace Tvision2.Engine.Components;

public readonly struct LayerSelector
{
    private static LayerSelector _top = new (byte.MaxValue);
    private static LayerSelector _bottom = new((byte)0);
    private static LayerSelector _standard = new ((byte)127);

    public static LayerSelector Top { get => _top; }
    public static LayerSelector Bottom { get => _bottom; }
    public static LayerSelector Standard { get => _standard; }

    public static LayerSelector FromIndex(byte index)
    {
        return new LayerSelector(index);
    }
    public byte LayerIndex { get; }
    private LayerSelector(byte layerIndex)
    {
        LayerIndex = layerIndex;
    }
}