namespace Dreadborne;

internal struct Projectile
{
    private readonly TextureRegion _texture;
    private Vector2 _position;

    public Projectile()
    {
        string path = "projectile-prefabs";
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, path);
        _texture = atlas.GetRegion("sword");
    }

    public readonly void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        _texture.Draw(spriteBatch, _position, Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
    }

    private Projectile(Vector2 velocity)
    {
        _position += velocity;
    }

    public static Projectile NewProjectile(Vector2 velocity)
    {
        return new Projectile(velocity);
    }
}