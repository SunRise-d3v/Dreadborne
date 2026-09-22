namespace Dreadborne.InputSystem;

internal static class HandleInput
{
    public static void MoveUpdate()
    {
        if (Key.IsKeyDown("Move_Left"))
            Player.Instance.Movement._direction.X -= 1;
        if (Key.IsKeyDown("Move_Right"))
            Player.Instance.Movement._direction.X += 1;

        if (Key.IsKeyDown("Move_Up"))
            Player.Instance.Movement._direction.Y -= 1;
        if (Key.IsKeyDown("Move_Down"))
            Player.Instance.Movement._direction.Y += 1;
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
    }
}