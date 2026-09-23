namespace Dreadborne.InputSystem;

internal static class HandleInput
{
    private static float CameraRotation = 0.05f;

    public static Vector2 GetMoveInput()
    {
        Vector2 direction = Vector2.Zero;

        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
            direction.X -= 1;
        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
            direction.X += 1;
        if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
            direction.Y -= 1;
        if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
            direction.Y += 1;

        return direction;
    }

    public static void AttackUpdate()
    {
        if (Player.Instance._shootTimer <= 0f && Input.Mouse.IsButtonDown(MouseButton.Left))
        {
            Vector2 target = Game.MainCamera.ScreenToWorld(Input.Mouse.Position.ToVector2());
            Projectile.NewProjectile(Player.Instance._movement.Position, target);
            Player.Instance._shootTimer = Player.SHOOT_COOLDOWN;
        }
    }

    public static void InventoryUpdate()
    {
        if (Key.IsKeyPressed("Inventory_Open"))
            Player.Instance.Inventory.IsOpen = !Player.Instance.Inventory.IsOpen;

        /*if (Player.Instance.Inventory.DragNDropField.Intersects(Input.Mouse.Bounds) && Input.Mouse.IsButtonDown(MouseButton.Left))
        {
            Player.Instance.Inventory._position = Input.Mouse.Position.ToVector2();
        }*/

        if (Input.Keyboard.IsKeyJustPressed(Keys.Z))
            Player.Instance.Inventory.AddItem(new Item("iron-shortsword", 1));

        if (Input.Keyboard.IsKeyJustPressed(Keys.X))
            Player.Instance.Inventory.RemoveItem("iron-shortsword", 1);
    }

    public static void CameraUpdate()
    {
        if (Input.Keyboard.IsKeyDown(Keys.Q))
            Game.MainCamera.Rotate(CameraRotation);

        if (Input.Keyboard.IsKeyDown(Keys.E))
            Game.MainCamera.Rotate(-CameraRotation);
    }
}