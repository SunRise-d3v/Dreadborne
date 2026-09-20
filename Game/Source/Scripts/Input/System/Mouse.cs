using static Microsoft.Xna.Framework.Input.Mouse;

namespace Dreadborne.InputSystem;

public enum MouseButton { Left, Middle, Right, XButton1, XButton2 };
public sealed class Mouse
{
    public MouseState PreviousState { get; private set; }
    public MouseState CurrentState { get; private set; }

    public Point Position
    {
        get => CurrentState.Position;
        set => SetPosition(value.X, value.Y);
    }

    public int X
    {
        get => CurrentState.X;
        set => SetPosition(value, CurrentState.Y);
    }

    public int Y
    {
        get => CurrentState.Y;
        set => SetPosition(CurrentState.X, value);
    }

    public Point PositionDelta => CurrentState.Position - PreviousState.Position;

    public int XDelta => CurrentState.X - PreviousState.X;
    public int YDelta => CurrentState.Y - PreviousState.Y;

    public bool WasMoved => PositionDelta != Point.Zero;

    public int ScrollWheel => CurrentState.ScrollWheelValue;
    public int ScrollWheelDelta => CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue;

    public Mouse()
    {
        PreviousState = new();
        CurrentState = GetState();
    }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = GetState();
    }

    public bool IsButtonDown(MouseButton button)
    {
        return button switch
        {
            MouseButton.Left => CurrentState.LeftButton == ButtonState.Pressed,
            MouseButton.Middle => CurrentState.MiddleButton == ButtonState.Pressed,
            MouseButton.Right => CurrentState.RightButton == ButtonState.Pressed,
            MouseButton.XButton1 => CurrentState.XButton1 == ButtonState.Pressed,
            MouseButton.XButton2 => CurrentState.XButton2 == ButtonState.Pressed,
            _ => false,
        };
    }

    public bool IsButtonUp(MouseButton button)
    {
        return button switch
        {
            MouseButton.Left => CurrentState.LeftButton == ButtonState.Released,
            MouseButton.Middle => CurrentState.MiddleButton == ButtonState.Released,
            MouseButton.Right => CurrentState.RightButton == ButtonState.Released,
            MouseButton.XButton1 => CurrentState.XButton1 == ButtonState.Released,
            MouseButton.XButton2 => CurrentState.XButton2 == ButtonState.Released,
            _ => false,
        };
    }

    public bool IsButtonJustPressed(MouseButton button)
    {
        return button switch
        {
            MouseButton.Left => CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released,
            MouseButton.Middle => CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released,
            MouseButton.Right => CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released,
            MouseButton.XButton1 => CurrentState.XButton1 == ButtonState.Pressed && PreviousState.XButton1 == ButtonState.Released,
            MouseButton.XButton2 => CurrentState.XButton2 == ButtonState.Pressed && PreviousState.XButton2 == ButtonState.Released,
            _ => false,
        };
    }

    public bool IsButtonJustReleased(MouseButton button)
    {
        return button switch
        {
            MouseButton.Left => CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed,
            MouseButton.Middle => CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed,
            MouseButton.Right => CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed,
            MouseButton.XButton1 => CurrentState.XButton1 == ButtonState.Released && PreviousState.XButton1 == ButtonState.Pressed,
            MouseButton.XButton2 => CurrentState.XButton2 == ButtonState.Released && PreviousState.XButton2 == ButtonState.Pressed,
            _ => false,
        };
    }

    public void SetCursorPosition(int x, int y)
    {
        SetPosition(x, y);
        CurrentState = new(x, y,
            CurrentState.ScrollWheelValue, CurrentState.LeftButton, CurrentState.MiddleButton,
            CurrentState.RightButton, CurrentState.XButton1, CurrentState.XButton2);
    }
}