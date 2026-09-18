namespace Dreadborne.Entity;

internal sealed class Player
{
    private readonly TextureRegion _texture;
    
    private Vector2 _position;
    public Vector2 Position => _position;

    private float _moveSpeed;
    private int _moveSpeedMultiplier;

    public Player(string pathFile, string field)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, pathFile);
        this._texture = atlas.GetRegion(field);

        _moveSpeed = 7f;
        _moveSpeedMultiplier = 20;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 direction = Vector2.Zero;

        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
            direction.X -= 1;
        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
            direction.X += 1;

        if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
            direction.Y -= 1;
        if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
            direction.Y += 1;

        if (direction != Vector2.Zero)
        {
            direction.Normalize();
            _position += direction * Move(_moveSpeed, deltaTime, _moveSpeedMultiplier);
        }
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        _texture.Draw(spriteBatch,_position,Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
    }

    private static float Move(float speed, float deltaTime, int multiplier) => speed * multiplier * deltaTime;
}