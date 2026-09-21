namespace Dreadborne.Scene;

internal sealed class GameScene : IScene
{
    private Player _player;
    private World _world;

    private Canvas _canvas;
    private Enemy _enemy1;
    private Enemy _enemy2;

    private List<Enemy> _enemies;

    public GameScene() { }

    public void Awake(ContentManager content)
    {
        _world = new(125, 125);

        _player = new(Utility.UploadXML("character-prefabs"), "white-man");
        
        _enemy1 = new(Utility.UploadXML("character-prefabs"), "gnome-mage", new(100, 100));
        _enemy2 = new(Utility.UploadXML("character-prefabs"), "gnome-mage", new(200, 100));

        _enemies = [];
        _enemies.Add(_enemy1);
        _enemies.Add(_enemy2);
    }

    public void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _player.Movement.SetPosition(new(_world.Width / 2, _world.Height / 2));

        _canvas = new(Game.GraphicsDevice, _player);
    }

    public void Update(GameTime gameTime)
    {
        MainCamera.Follow(_player.Movement.Position);
        _player.Update(gameTime);

        Projectile.UpdateAll(gameTime, _enemies);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _world.Draw(spriteBatch);

        foreach (Enemy enemy in _enemies)
            enemy.Draw(spriteBatch, Layer.EntityLayer);

        _player.Draw(spriteBatch, Layer.PlayerLayer);

        Projectile.DrawAll(spriteBatch, Layer.EntityLayer);
    }

    public void DrawGUI(SpriteBatch spriteBatch)
    {
        _canvas.Draw(spriteBatch, Layer.GUILayer);
    }
}