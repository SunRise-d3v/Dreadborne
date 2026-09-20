namespace Dreadborne.Graphics;

public sealed class TextureRegion
{
    public Texture2D Texture { get; }

    public Rectangle SourceRectangle { get; set; }

    public int Width => SourceRectangle.Width;

    public int Height => SourceRectangle.Height;

    public TextureRegion() { }

    public TextureRegion(Texture2D texture, int x, int y, int width, int height)
    {
        Texture = texture;
        SourceRectangle = new(x, y, width, height);
    }

    /*public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        Draw(spriteBatch, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }*/

    /*public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
    {
        Draw(
            spriteBatch,
            position,
            color,
            rotation,
            origin,
            new Vector2(scale, scale),
            effects,
            layerDepth
        );
    }*/

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects effects, float layerDepth)
        => spriteBatch.Draw(Texture, position, SourceRectangle, color, rotation, CentrePivot(), scale, effects, layerDepth);

    private Vector2 CentrePivot() => new Vector2(Width, Height) * 0.5f;
    public static Vector2 ChangeOffset(Vector2 offset) => offset;
}