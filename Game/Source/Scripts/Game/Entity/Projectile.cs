using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne.Entity;

internal struct Projectile
{
    private const float SPEED = 4f;
    private const int MULTIPLIUR = 30;
    private const float LIFETIME = 3f;
    private const float ROTATION_OFFSET = 40f;

    private static readonly List<Projectile> _projectiles = [];
    private static TextureRegion s_texture;

    private Vector2 _position;
    private readonly Vector2 _direction;
    private readonly float _rotation;
    private float _timeLeft = LIFETIME;
    private readonly SpriteEffects _flip;
    private OrientedRectangle _hitbox;

    private Projectile(Vector2 position, Vector2 direction)
    {
        _position = position;
        _direction = Vector2.Normalize(direction);
        _rotation = MathF.Atan2(_direction.Y, _direction.X) + ROTATION_OFFSET;
        _flip = _direction.X < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

        _hitbox = new(_position, new Vector2(s_texture.Width, s_texture.Height), _rotation);
    }

    public static void NewProjectile(Vector2 owner, Vector2 target)
    {
        Vector2 direction = target - owner;
        if (direction == Vector2.Zero)
            return;

        if (s_texture == null)
        {
            string path = Utility.UploadXML("projectile-prefabs");
            s_texture = TextureAtlas.FromFile(Game.Content, path).GetRegion("sword");
        }

        _projectiles.Add(new(owner, direction));
    }

    public static void UpdateAll(GameTime gameTime, List<Enemy> enemies)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (ref Projectile p in CollectionsMarshal.AsSpan(_projectiles))
        {
            p._position += p._direction * (SPEED * MULTIPLIUR) * deltaTime;
            p._hitbox = new(p._position, new Vector2(s_texture.Width, s_texture.Height), p._rotation);

            foreach (Enemy enemy in enemies)
            {
                if (p._hitbox.Intersects(OrientedRectangle.FromRectangle(enemy.Hitbox)))
                {
                    enemy.TakeDamage(5);
                    _projectiles.Remove(p);
                }
            }

            p._timeLeft -= deltaTime;
        }

        _projectiles.RemoveAll(static p => p._timeLeft <= 0f);
    }

    public static void DrawAll(SpriteBatch spriteBatch, Layer layer)
    {
        foreach (Projectile p in _projectiles)
        {
            s_texture.Draw(spriteBatch, p._position, Color.White, p._rotation, Vector2.One, p._flip, layer.Depth);
            Debug.DrawOrientedRectangleBorder(spriteBatch, p._hitbox, Color.Red, 1);
        }
    }
}