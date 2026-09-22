namespace Dreadborne;

internal sealed class PlayerMovement
{
    private Vector2 _position;
    public Vector2 Position => _position;

    internal Vector2 _direction;

    private float _moveSpeed;
    private int _moveSpeedMultiplier;

    public PlayerMovement()
    {
        _moveSpeed = 7f;
        _moveSpeedMultiplier = 20;
        _moveSpeedMultiplier = 100;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _direction = Vector2.Zero;

        HandleInput.MoveUpdate();

        if (_direction != Vector2.Zero)
        {
            _direction.Normalize();
            _position += _direction * Move(_moveSpeed, deltaTime, _moveSpeedMultiplier);
        }
    }

    public void SetPosition(Vector2 position) => _position = new(position.X * Tile.TILE_SIZE, position.Y * Tile.TILE_SIZE);

    private static float Move(float speed, float deltaTime, int multiplier) => (speed * multiplier) * deltaTime;
}