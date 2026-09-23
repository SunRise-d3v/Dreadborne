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
        _moveSpeedMultiplier = 200;
        //_moveSpeedMultiplier = 100;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 inputDirection = HandleInput.GetMoveInput();

        if (inputDirection != Vector2.Zero)
        {
            inputDirection.Normalize();

            Vector2 worldDirection = RotateVector(inputDirection, MainCamera.SpriteRotate);
            _position += worldDirection * Move(_moveSpeed, deltaTime, _moveSpeedMultiplier);
        }
    }

    private static Vector2 RotateVector(Vector2 vector, float radians)
    {
        float cos = MathF.Cos(radians);
        float sin = MathF.Sin(radians);

        return new Vector2(
            vector.X * cos - vector.Y * sin,
            vector.X * sin + vector.Y * cos);
    }

    public void SetPosition(Vector2 position) => _position = new(position.X * Tile.TILE_SIZE, position.Y * Tile.TILE_SIZE);

    private static float Move(float speed, float deltaTime, int multiplier) => (speed * multiplier) * deltaTime;
}