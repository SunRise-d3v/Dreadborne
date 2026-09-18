namespace Dreadborne.DeveloperTools;

internal sealed class FPSCounter
{
    private int _fps;
    public int FPS => _fps;

    private int _frames;
    private double _seconds;

    [Conditional("DEBUG")]
    public void Update(GameTime gameTime)
    {
        _seconds += gameTime.ElapsedGameTime.TotalSeconds;

        if (_seconds >= 1)
        {
            _fps = _frames;
            _seconds = 0;
            _frames = 0;
        }
    }

    [Conditional("DEBUG")]
    public void UpdateFrame() => _frames++;
}