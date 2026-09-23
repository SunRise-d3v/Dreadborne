namespace Dreadborne.UI;

internal sealed class EnemyHUD
{
    private Texture2D _pixel;

    private readonly string _name;
    private int _health, _maxHealth;
    private TextureRegion _texture;

    private Rectangle _rectangle;
    private Vector2 _position;
    private readonly byte _w, _h;

    private bool _active;

    public EnemyHUD(Enemy enemy)
    {
        _pixel = new(Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _name = "Gnome";
        _health = enemy.Health;
        _maxHealth = enemy.MaxHealth;

        _texture = enemy.Texture;

        _w = 45;
        _h = 30;

        _position = new(enemy.Position.X, enemy.Position.Y - 75);
        _rectangle = new((int)_position.X, (int)_position.Y, _w, _h);

        _active = true;
    }

    public void Update(Enemy enemy)
    {
        _health = enemy.Health;
        _maxHealth = enemy.MaxHealth;
    }

    public void Display(SpriteBatch spriteBatch, Layer layer)
    {
        if (_active)
        {
            spriteBatch.Draw(_pixel, _rectangle, _rectangle, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, layer.Depth);
            spriteBatch.DrawString(Canvas.DefaultFont, $"{_name}", _position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer.Depth + 0.01f);
            spriteBatch.DrawString(Canvas.DefaultFont, $"{_health}/{_maxHealth}", _position + new Vector2(0, 12), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer.Depth + 0.01f);
        }
    }

    public bool Destroy()
    {
        _active = false;

        _rectangle = Rectangle.Empty;
        _texture = null;
        _pixel = null;
        return true;
    }
}