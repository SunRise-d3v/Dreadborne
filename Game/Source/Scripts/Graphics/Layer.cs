namespace Dreadborne.Graphics;

public readonly struct Layer
{
    private static readonly Dictionary<string, Layer> _layers = new();

    public string Name { get; }
    public float Depth { get; }

    #region Default Game Layers

    public static readonly Layer FloorLayer = new("floor", 0.0f);
    public static readonly Layer WallLayer = new("wall", 0.1f);
    public static readonly Layer FurnitureLayer = new("furniture", 0.15f);
    public static readonly Layer ItemLayer = new("item", 0.2f);
    public static readonly Layer BackLayer = new("back", 0.3f);
    public static readonly Layer EntityLayer = new("entity", 0.33f);
    public static readonly Layer PlayerLayer = new("player", 0.35f);
    public static readonly Layer AccessoryLayer = new("accessory", 0.4f);
    public static readonly Layer DoorLayer = new("door", 0.5f);
    public static readonly Layer GUILayer = new("gui", 0.6f);
    public static readonly Layer TransitionLayer = new("transition", 0.8f);
    public static readonly Layer UILayer = new("ui", 0.9f);

    #endregion

    private Layer(string name, float depth)
    {
        Name = name;
        Depth = depth;

        _layers.Add(name, this);
    }
}