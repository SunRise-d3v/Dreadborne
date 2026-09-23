using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne.Entity;

internal sealed class Player
{
    #region Player
    private static Player p_instance;
    public static Player Instance => p_instance;
    #endregion

    private readonly TextureRegion _texture;

    internal readonly PlayerMovement _movement;
    public PlayerMovement Movement => _movement;

    //private WeakAttack _attack;

    public const float SHOOT_COOLDOWN = 0.3f;
    internal float _shootTimer;

    private Rectangle _hitbox, _collider;
    public Rectangle Collider => _collider;

    private readonly Bar _healthBar;
    private readonly Bar _manaBar;
    public Bar HealthBar => _healthBar;
    public Bar ManaBar => _manaBar;

    private readonly Inventory _inventory;
    public Inventory Inventory => _inventory;

    public Player(string pathFile, string field)
    {
        p_instance = this;

        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, pathFile);
        this._texture = atlas.GetRegion(field);

        _movement = new();
        //_attack = new();

        _inventory = new(Game.Content, Game.GraphicsDevice);

        _healthBar = new(new(Game.SCREEN_WIDTH / 2 - 1, Game.SCREEN_HEIGHT - 18 * 2), 300, 20);
        _manaBar = new(new(Game.SCREEN_WIDTH / 2 - 1, Game.SCREEN_HEIGHT - 16), 300, 20);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        HandleInput.InventoryUpdate();
        _movement.Update(gameTime);

        _hitbox = new((int)_movement.Position.X - _texture.Width / 4, (int)_movement.Position.Y - _texture.Height / 5, _texture.Width / 2, _texture.Height / 3);
        _collider = new((int)_movement.Position.X - _texture.Width / 4, (int)_movement.Position.Y, _texture.Width / 2, _texture.Height / 2);

        _shootTimer -= deltaTime;
        HandleInput.AttackUpdate();

        HandleInput.CameraUpdate() ;
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        //_inventory.Draw(spriteBatch);
        _texture.Draw(spriteBatch, _movement.Position, Color.White, Game.MainCamera.SpriteRotate, Vector2.One, SpriteEffects.None, layer.Depth);
        
        //_healthBar.Draw(spriteBatch,Layer.GUILayer, _movement.Position,13,20,Color.Red, Color.Green);

        Debug.DrawRectangleBorder(spriteBatch, _collider, Debug.Collider, 1);
        Debug.DrawRectangleBorder(spriteBatch, _hitbox, Debug.DynamicObject, 1);
        //_attack.Draw(spriteBatch);
    }
}