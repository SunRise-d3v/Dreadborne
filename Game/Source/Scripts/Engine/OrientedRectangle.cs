namespace Dreadborne;

public readonly struct OrientedRectangle(Vector2 center, Vector2 size, float rotation)
{
    public Vector2 Center { get; } = center;
    public Vector2 HalfSize { get; } = size * 0.5f;
    public float Rotation { get; } = rotation;

    public static OrientedRectangle FromRectangle(Rectangle rect)
        => new(rect.Center.ToVector2(), rect.Size.ToVector2(), 0f);

    public static OrientedRectangle Empty => new(Vector2.Zero, Size.Zero, 0f);

    private Vector2 AxisX => new(MathF.Cos(Rotation), MathF.Sin(Rotation));
    private Vector2 AxisY => new(-MathF.Sin(Rotation), MathF.Cos(Rotation));

    public void GetCorners(Span<Vector2> corners)
    {
        Vector2 x = AxisX * HalfSize.X;
        Vector2 y = AxisY * HalfSize.Y;

        corners[0] = Center - x - y;
        corners[1] = Center + x - y;
        corners[2] = Center + x + y;
        corners[3] = Center - x + y;
    }

    public bool Intersects(OrientedRectangle other)
    {
        Vector2 offset = other.Center - Center;

        Span<Vector2> axes = [AxisX, AxisY, other.AxisX, other.AxisY];
        foreach (Vector2 axis in axes)
        {
            float distance = MathF.Abs(Vector2.Dot(offset, axis));
            if (distance > Radius(axis) + other.Radius(axis))
                return false;
        }

        return true;
    }

    private float Radius(Vector2 axis)
        => HalfSize.X * MathF.Abs(Vector2.Dot(AxisX, axis))
         + HalfSize.Y * MathF.Abs(Vector2.Dot(AxisY, axis));
}