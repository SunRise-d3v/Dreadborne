namespace Dreadborne.Graphics;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class SpriteAttribute : Attribute
{
    public string File { get; }
    public string SpriteName { get; }

    public SpriteAttribute(string file, string spriteName)
    {
        File = file;
        SpriteName = spriteName;
    }
}