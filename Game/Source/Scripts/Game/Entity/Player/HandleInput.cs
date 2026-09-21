namespace Dreadborne.InputSystem;

internal static class HandleInput
{
    public static void Update(ref Vector2 direction)
    {
        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
            direction.X -= 1;
        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
            direction.X += 1;

        if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
            direction.Y -= 1;
        if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
            direction.Y += 1;
    }
}