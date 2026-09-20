namespace Dreadborne.Entity;

internal sealed class Player
{
    private readonly TextureRegion _texture;

    private readonly PlayerMovement _movement;
    public PlayerMovement Movement => _movement;

    //private WeakAttack _attack;

    public Player(string pathFile, string field)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, pathFile);
        this._texture = atlas.GetRegion(field);

        _movement = new();
        //_attack = new();
    }

    public void Update(GameTime gameTime)
    {
        _movement.Update(gameTime);

        if (Input.Mouse.IsButtonDown(MouseButton.Left))
        {
            //_attack.Attack();
            Projectile.NewProjectile(new(1, 1));
        }
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        _texture.Draw(spriteBatch, _movement.Position, Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
        //_attack.Draw(spriteBatch);
    }
}