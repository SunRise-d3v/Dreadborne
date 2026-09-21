using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne.Entity;

internal sealed class Player
{
    private readonly TextureRegion _texture;

    private readonly PlayerMovement _movement;
    public PlayerMovement Movement => _movement;

    //private WeakAttack _attack;

    private const float SHOOT_COOLDOWN = 0.3f;
    private float _shootTimer;

    private Rectangle _hitbox, _collider;

    private readonly HealthBar _healthBar;
    public HealthBar HealthBar => _healthBar;

    public Player(string pathFile, string field)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, pathFile);
        this._texture = atlas.GetRegion(field);

        _movement = new();
        //_attack = new();
        _healthBar = new(new(Game.SCREEN_WIDTH / 2 - 1, Game.SCREEN_HEIGHT - 16), 300, 20);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _movement.Update(gameTime);

        _hitbox = new((int)_movement.Position.X - _texture.Width / 4, (int)_movement.Position.Y - _texture.Height / 5, _texture.Width / 2, _texture.Height / 3);
        _collider = new((int)_movement.Position.X - _texture.Width / 4, (int)_movement.Position.Y, _texture.Width / 2, _texture.Height / 2);

        _shootTimer -= deltaTime;

        if (_shootTimer <= 0f && Input.Mouse.IsButtonDown(MouseButton.Left))
        {
            Vector2 target = Game.MainCamera.ScreenToWorld(Input.Mouse.Position.ToVector2());
            Projectile.NewProjectile(_movement.Position, target);
            _shootTimer = SHOOT_COOLDOWN;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        _texture.Draw(spriteBatch, _movement.Position, Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
        //_healthBar.Draw(spriteBatch,Layer.GUILayer, _movement.Position,13,20,Color.Red, Color.Green);

        Debug.DrawRectangleBorder(spriteBatch, _collider, Color.White, 1);
        Debug.DrawRectangleBorder(spriteBatch, _hitbox, Color.Red, 1);
        //_attack.Draw(spriteBatch);
    }
}