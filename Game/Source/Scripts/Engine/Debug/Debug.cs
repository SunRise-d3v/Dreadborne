namespace Dreadborne.DeveloperTools;

public static class Debug
{
    private static Texture2D _pixel;

    private static bool _visible;

    public static bool IsOpen;

    public static readonly Color StaticObject = Color.Blue;
    public static readonly Color DynamicObject = Color.Red;
    public static readonly Color Collider = Color.White;
    public static readonly Color InteractionObject = Color.LawnGreen;
    //public static readonly Color DynamicObject = Color.Red;

    [Conditional("DEBUG")]
    public static void Init(GraphicsDevice graphicsDevice)
    {
        IsOpen = false;
        _visible = true;

        _pixel = new(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    [Conditional("DEBUG")]
    public static void Draw(GameTime gameTime)
    {
        Core.ImGuiRenderer.BeforeLayout(gameTime);

        if (IsOpen)
        {
            ImGui.Begin("Debug Window", ref IsOpen);

            ImGui.Checkbox("Visible Hitbox", ref _visible);

            ImGui.End();
        }

        Core.ImGuiRenderer.AfterLayout();
    }

    [Conditional("DEBUG")]
    public static void DrawRectangleBorder(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness = 2)
    {
        Rectangle up = new(rect.X, rect.Y, rect.Width, thickness);
        Rectangle left = new(rect.X, rect.Bottom - thickness, rect.Width, thickness);
        Rectangle right = new(rect.X, rect.Y, thickness, rect.Height);
        Rectangle down = new(rect.Right - thickness, rect.Y, thickness, rect.Height);

        float layerDepth = Layer.UILayer.Depth;

        if (_visible)
        {
            spriteBatch.Draw(_pixel, up, up, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            spriteBatch.Draw(_pixel, left, left, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            spriteBatch.Draw(_pixel, right, right, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            spriteBatch.Draw(_pixel, down, down, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
        }
    }

    [Conditional("DEBUG")]
    public static void DrawOrientedRectangleBorder(SpriteBatch spriteBatch, OrientedRectangle rect, Color color, int thickness = 2, float layerDepth = 0.95f)
    {
        if (!_visible)
            return;

        Span<Vector2> corners = stackalloc Vector2[4];
        rect.GetCorners(corners);

        for (int i = 0; i < 4; i++)
        {
            Vector2 from = corners[i];
            Vector2 edge = corners[(i + 1) % 4] - from;
            float angle = MathF.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(_pixel, from, null, color, angle, Vector2.Zero,
                new Vector2(edge.Length(), thickness), SpriteEffects.None, layerDepth);
        }
    }
}