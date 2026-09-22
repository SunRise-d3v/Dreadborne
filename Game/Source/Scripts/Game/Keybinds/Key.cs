namespace Dreadborne.InputSystem;

internal static class Key
{
    private static readonly Dictionary<string, Keys[]> _bindings = new()
    {
        ["Move_Up"] = [Keys.W, Keys.Up],
        ["Move_Down"] = [Keys.S, Keys.Down],
        ["Move_Left"] = [Keys.A, Keys.Left],
        ["Move_Right"] = [Keys.D, Keys.Right],

        ["Inventory_Open"] = [Keys.I]
    };

    public static bool IsKeyDown(string action)
    {
        foreach (Keys key in _bindings[action])
            if (Input.Keyboard.IsKeyDown(key))
                return true;

        return false;
    }

    public static bool IsKeyPressed(string action)
    {
        foreach (Keys key in _bindings[action])
            if (Input.Keyboard.IsKeyJustPressed(key))
                return true;

        return false;
    }

    public static void RebindKey(string action, Keys[] newKey)
    {
        if (!_bindings.ContainsKey(action))
            throw new ArgumentException($"Unknown action: {action}", nameof(action));

        _bindings[action] = newKey;
    }
}