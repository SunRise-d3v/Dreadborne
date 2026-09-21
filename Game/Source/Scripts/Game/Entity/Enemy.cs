using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne.Entity;

internal class Enemy
{
    private TextureRegion _texture;
    private Vector2 _position;

    private Rectangle _hitbox, _collider;
    public Rectangle Hitbox => _hitbox;

    private ushort _health, _maxHealth;
    private bool _isAlive;

    public Enemy(string pathFile, string field, Vector2 position)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, pathFile);
        this._texture = atlas.GetRegion(field);

        this._position = position;
        //_collider = new((int)_position.X - _texture.Width / 4, (int)_position.Y, _texture.Width / 2, _texture.Height / 2);

        _hitbox = new((int)_position.X - _texture.Width / 4, (int)_position.Y - _texture.Height / 5, _texture.Width / 3, _texture.Height / 2);

        _health = _maxHealth = 20;
        _isAlive = true;
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        if (_isAlive)
        {
            _texture.Draw(spriteBatch, _position, Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
            Debug.DrawRectangleBorder(spriteBatch, _hitbox, Color.Red, 1);
        }
    }

    public ushort TakeDamage(ushort damage)
    {
        if (_health > 0)
        {
            _health -= damage;
            return damage;
        }
        else
        {
            Destroy(this);
            return 0;
        }
    }

    public static void Destroy(Enemy enemy)
    {
        enemy._texture = null;
        enemy._hitbox = Rectangle.Empty;
        enemy._isAlive = false;
    }
}