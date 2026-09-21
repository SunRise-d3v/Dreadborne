namespace Dreadborne.UI;

internal sealed class Bar
{
    private readonly Texture2D _pixel;

    private Vector2 _position;
    private Rectangle _bar;

    public Bar(Vector2 position, int width, int height)
    {
        _pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _position = position;
        _bar = new((int)_position.X, (int)_position.Y, width, height);
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer, int currentHealth, int maxHealth, Color backColor, Color fillColor)
    {
        int barWidth = _bar.Width;
        int barHeight = _bar.Height;

        int healthBarX = (int)_position.X + (Tile.TILE_SIZE - barWidth) / 2;
        int healthBarY = (int)_position.Y - barHeight - 2;
        
        float healthPercentage = (float)currentHealth / maxHealth;
        int filledWidth = (int)(barWidth * healthPercentage);

        Vector2 origin = new((barWidth - 2) / 2, -Tile.TILE_SIZE / 2 + barHeight);

        //spriteBatch.Draw(_pixel, position, new Rectangle(healthBarX, healthBarY, barWidth, barHeight), backColor * 0.45f, 0f, origin, 1f, SpriteEffects.None, layer.Depth);
        //spriteBatch.Draw(_pixel, position, new Rectangle(healthBarX, healthBarY, filledWidth, barHeight), fillColor, 0f, origin, 1f, SpriteEffects.None, layer.Depth + 0.01f);

        spriteBatch.Draw(_pixel, _position, new Rectangle(healthBarX, healthBarY, barWidth, barHeight), backColor * 0.45f, 0f, origin, 1f, SpriteEffects.None, layer.Depth);
        spriteBatch.Draw(_pixel, _position, new Rectangle(healthBarX, healthBarY, filledWidth, barHeight), fillColor, 0f, origin, 1f, SpriteEffects.None, layer.Depth + 0.01f);
    }
}