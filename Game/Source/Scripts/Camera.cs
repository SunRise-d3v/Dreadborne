namespace Dreadborne;

public sealed class Camera(Viewport viewport)
{
    private readonly Viewport _viewport = viewport;

    private Vector2 _position = Vector2.Zero;
    public Vector2 Position => _position;

    public float Zoom { get; private set; } = 1f;
    public float Rotation { get; } = 0f;

    public Matrix Transform =>
        Matrix.CreateTranslation(new Vector3(-_position, 0f)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(Zoom, Zoom, 1f) *
        Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0f));

    public void Follow(Vector2 target)
    {
        _position = target;
    }

    public void SetZoom(float addZoom)
    {
        this.Zoom = addZoom;
    }
}