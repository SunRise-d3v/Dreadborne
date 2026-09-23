using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne.Entity;

internal class Enemy
{
    private TextureRegion _texture;
    private Vector2 _position;
    public Vector2 Position => _position;

    private Rectangle _hitbox, _collider;
    public Rectangle Hitbox => _hitbox;

    private ushort _health, _maxHealth;
    public ushort Health => _health;
    public ushort MaxHealth => _maxHealth;

    private bool _isAlive;

    private readonly DroppedLoot _loot;
    internal DroppedLoot Loot => _loot;
    internal TextureRegion Texture => _texture;

    private readonly EnemyHUD _hud;
    private bool _target;

    public Enemy(string pathFile, string field, Vector2 position)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, pathFile);
        this._texture = atlas.GetRegion(field);

        this._position = position;

        _collider = new((int)_position.X - _texture.Width / 4, (int)_position.Y, _texture.Width / 2, _texture.Height / 2);
        _hitbox = new((int)_position.X - _texture.Width / 4, (int)_position.Y - _texture.Height / 5, _texture.Width / 3, _texture.Height / 2);

        _health = _maxHealth = 20;
        _isAlive = true;

        _loot = new(new Item("iron-shortsword", 1, staking: false));

        _hud = new(this);
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        if (_health != _maxHealth || _health < _maxHealth)
            _target = true;

        if (_isAlive)
        {
            _texture.Draw(spriteBatch, _position, Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);

            if (_target)
                _hud.Display(spriteBatch, Layer.GUILayer);

            Debug.DrawRectangleBorder(spriteBatch, _collider, Debug.Collider, 1);
            Debug.DrawRectangleBorder(spriteBatch, _hitbox, Debug.DynamicObject, 1);
        }
        else
        {
            _loot?.Draw(spriteBatch);
            if (_loot.Rectangle.Intersects(Player.Instance.Collider))
            {
                Player.Instance.Inventory.AddItem(_loot.Loots);
                _loot.Destroy();
            }
        }
    }

    public ushort TakeDamage(ushort damage)
    {
        _hud.Update(this);
        if (_health > 0)
        {
            _health -= damage;
            return damage;
        }
        else
        {
            _hud.Destroy();
            SpawnLoot();
            return 0;
        }
    }

    private void SpawnLoot()
    {
        _loot.Drop(this);
        Destroy();
    }

    public void Destroy()
    {
        _texture = null;
        _hitbox = Rectangle.Empty;
        _isAlive = false;
    }

    public void SetPosition(Vector2 position)
    {
        this._position = position;
    }
}