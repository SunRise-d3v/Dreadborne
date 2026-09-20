namespace Dreadborne.Scene;

internal sealed class GameScene : IScene
{
    private Player _player;
    private World _world;

    private Canvas _canvas;

    private const byte MAX_PROJECTILES = byte.MaxValue;
    private Projectile[] _projectiles;

    public GameScene() { }

    public void Awake(ContentManager content)
    {
        _projectiles = new Projectile[MAX_PROJECTILES];

        _world = new(125, 125);

        _player = new(Utility.UploadXML("character-prefabs"), "white-man");
    }

    public void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _player.Movement.SetPosition(new(_world.Width / 2, _world.Height / 2));
        
        _canvas = new(Game.GraphicsDevice);
    }

    public void Update(GameTime gameTime)
    {
        MainCamera.Follow(_player.Movement.Position);
        _player.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _world.Draw(spriteBatch);

        _player.Draw(spriteBatch, Layer.PlayerLayer);
    }

    public void DrawGUI(SpriteBatch spriteBatch)
    {
        _canvas.Draw(spriteBatch, Layer.GUILayer);
    }
}