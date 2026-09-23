using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne;

public class Core : Microsoft.Xna.Framework.Game
{
    private static Core s_instance;

    public static Core Instance => s_instance;

    public static GraphicsDeviceManager Graphics { get; private set; }

    public static new GraphicsDevice GraphicsDevice { get; private set; }

    public static SpriteBatch SpriteBatch { get; private set; }

    public static new ContentManager Content { get; private set; }

    public static ImGuiRenderer ImGuiRenderer { get; private set; }

    public Cursor Cursor { get; private set; }

    private readonly FPSCounter _fpsCounter;
    private readonly string _title;
    private readonly int _width, _height;

    public Core(string title, int width, int height, bool fullScreen, bool fixedStep = true, bool vSync = true)
    {
        if (s_instance != null)
            throw new InvalidOperationException($"Only a single Core instance can be created");

        s_instance = this;

        _title = title;

        _width = width;
        _height = height;

        CreateWindow(_width, _height, fullScreen);

        Content = base.Content;

        Content.RootDirectory = "Source";

        _fpsCounter = new();

        IsFixedTimeStep = fixedStep;
        Graphics.SynchronizeWithVerticalRetrace = vSync;
        //TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / _fpsCounter.FPS);

        Window.AllowUserResizing = true;

        Graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        PreInitialize();

        base.Initialize();

        LateInitialize();
    }

    protected virtual void PreInitialize()
    {
        IsMouseVisible = false;
        //IsMouseVisible = true;
        Cursor = new(Utility.UploadXML("icon-prefabs"), "cursor");
    }

    protected virtual void LateInitialize()
    {
        ImGuiRenderer = new ImGuiRenderer((Game)this);
        ImGuiRenderer.RebuildFontAtlas();

        var io = ImGui.GetIO();
        io.FontGlobalScale = 1.125f;
        ImGui.GetStyle().ScaleAllSizes(1.05f);

        Debug.Init(GraphicsDevice);
        Console.Write("\nGame started!\n");
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update();
        Cursor.Update();

#if DEBUG
        if (Input.Keyboard.IsKeyJustPressed(Keys.OemTilde))
        {
            Debug.IsOpen = !Debug.IsOpen;
            IsMouseVisible = Debug.IsOpen;
        }
#endif

        _fpsCounter.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);

        DrawGame(gameTime);
        DrawUI(gameTime);

        _fpsCounter.UpdateFrame();
    }

    protected virtual void DrawGame(GameTime gameTime)
    {
#if DEBUG
        Window.Title = $"Title: [{_title}] | Resolution: [{_width}x{_height}] | FPS: [{_fpsCounter.FPS}]";
#endif
    }

    protected virtual void DrawUI(GameTime gameTime)
    {
        Debug.Draw(gameTime);
    }

    private void CreateWindow(int width, int height, bool fullScreen)
    {
        Graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = fullScreen
        };

        Window.Title = _title;
    }
}