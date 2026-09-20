namespace Dreadborne.Graphics;

public class Sprite
{
    public TextureRegion Region { get; set; }

    public Color Color { get; set; } = Color.White;

    public float Rotation { get; set; } = 0.0f;

    public Vector2 Scale { get; set; } = Vector2.One;

    public Vector2 Origin { get; set; } = Vector2.Zero;

    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    public float LayerDepth { get; set; } = 0.0f;

    public float Width => Region.Width * Scale.X;

    public float Height => Region.Height * Scale.Y;

    public Sprite() { CenterOrigin(); }

    public Sprite(TextureRegion region)
    {
        Region = region;
        CenterOrigin();
    }

    public void CenterOrigin()
    {
        Origin = new Vector2(Region.Width, Region.Height) * 0.5f;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Layer layer)
    {
        Region.Draw(spriteBatch, position, Color, Rotation, Scale, Effects, layer.Depth);
    }

    public void ChangeTexture(string file, string path)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, file);
        Region = atlas.GetRegion(path);
    }
}