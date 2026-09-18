namespace Dreadborne;

public sealed class Game : Core
{
    public const ushort SCREEN_WIDTH = 800;
    public const ushort SCREEN_HEIGHT = 608;
    private const string GAME_TITLE = "Dread/Borne";

    private readonly SceneManager _sceneManager;

    public static Camera MainCamera;

    public Game() : base(GAME_TITLE, SCREEN_WIDTH, SCREEN_HEIGHT, false) => _sceneManager = new();

    protected override void PreInitialize()
    {
        base.PreInitialize();
    }

    protected override void LateInitialize()
    {
        base.LateInitialize();

        MainCamera = new(GraphicsDevice.Viewport);
        MainCamera.SetZoom(1.75f);

        _sceneManager.GetCurrentScene().Load(GraphicsDevice, Content);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        _sceneManager.AddScene(new GameScene());
    }

    protected override void Update(GameTime gameTime)
    {
        _sceneManager.GetCurrentScene().Update(gameTime);

        base.Update(gameTime);
    }

    protected override void DrawGame(GameTime gameTime)
    {
        base.DrawGame(gameTime);

        SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, samplerState: SamplerState.PointClamp, transformMatrix: MainCamera.Transform);

        _sceneManager.GetCurrentScene().Draw(SpriteBatch);

        SpriteBatch.End();
    }

    protected override void DrawUI(GameTime gameTime)
    {
        base.DrawUI(gameTime);

        SpriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

        _sceneManager.GetCurrentScene().DrawGUI(SpriteBatch);
        Cursor.Draw(SpriteBatch, Layer.UILayer);

        SpriteBatch.End();
    }
}