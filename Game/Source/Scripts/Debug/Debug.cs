namespace Dreadborne.DeveloperTools;

public static class Debug
{
    private static Texture2D _pixel;

    private static bool _visible;

    public static bool IsOpen;

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
        if (_visible)
        {
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}