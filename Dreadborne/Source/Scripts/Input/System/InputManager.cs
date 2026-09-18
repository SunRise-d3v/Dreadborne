namespace Dreadborne.InputSystem;

public static class Input
{
    public static Keyboard Keyboard { get; private set; } = new();
    public static Mouse Mouse { get; private set; } = new();

    public static void Update()
    {
        Keyboard.Update();
        Mouse.Update();
    }
}