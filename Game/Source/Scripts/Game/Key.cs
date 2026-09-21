namespace Dreadborne.InputSystem;

internal static class Key
{
    public static char KEY_DIND_MOVE_UP = 'W';
    public static char KEY_DIND_MOVE_RIGHT = 'D';
    public static char KEY_DIND_MOVE_LEFT = 'A';
    public static char KEY_DIND_MOVE_TOP = 'S';

    public static char RedindKey(char standartKey, char newKey)
    {
        return standartKey = newKey;
    }
}