using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne.Scene;

internal sealed class GameScene : IScene
{
    private Player _player;
    private World _world;

    private Canvas _canvas;
    private Enemy _enemy1;
    private Enemy _enemy2;

    private List<Enemy> _enemies;

    private Model _cube;

    private Vector3 _cameraTarget;
    private Vector3 _cameraPosition;

    private Matrix _projectionMatrix;
    private Matrix _viewMatrix;
    private Matrix _worldMatrix;

    public GameScene() { }

    public void Awake(ContentManager content)
    {
        _world = new(125, 125);

        _player = new(Utility.UploadXML("character-prefabs"), "white-man");

        _enemy1 = new(Utility.UploadXML("character-prefabs"), "gnome-mage", new(125 / 2 * 32, 125 / 2 * 32 - 100));
        _enemy2 = new(Utility.UploadXML("character-prefabs"), "gnome-mage", new(200, 100));

        _enemies = [];
        _enemies.Add(_enemy1);
        _enemies.Add(_enemy2);

        _cameraTarget = new(0f, 0f, 0f);
        _cameraPosition = new(0f, 0f, -10f);
    }

    public void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _player.Movement.SetPosition(new(_world.Width / 2, _world.Height / 2));
        _enemy1.SetPosition(new Vector2(_world.Width / 2, _world.Height / 2 - 100));

        _canvas = new(Game.GraphicsDevice);

        DebugConsole.Log($"Scene: \'Game\' loaded", ConsoleColor.White, ConsoleColor.Green);

        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView
            (MathHelper.ToRadians(45f),
            Game.GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);

        _viewMatrix = Matrix.CreateLookAt(_cameraPosition, _cameraTarget, Vector3.Up);

        _worldMatrix = Matrix.CreateWorld(_cameraTarget, Vector3.Forward, Vector3.Up);

        //_cube = content.Load<Model>("Resources/3D/Model");
    }

    public void Update(GameTime gameTime)
    {
        MainCamera.Follow(_player.Movement.Position);
        _player.Update(gameTime);

        Projectile.UpdateAll(gameTime, _enemies);

        if (true)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
            _cameraPosition = Vector3.Transform(_cameraPosition, rotationMatrix);
        }

        _viewMatrix = Matrix.CreateLookAt(_cameraPosition, _cameraTarget, Vector3.Up);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _world.Draw(spriteBatch);

        foreach (Enemy enemy in _enemies)
            enemy.Draw(spriteBatch, Layer.EntityLayer);

        _player.Draw(spriteBatch, Layer.PlayerLayer);

        Projectile.DrawAll(spriteBatch, Layer.EntityLayer);

        /*foreach (ModelMesh mesh in _cube.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects.Cast<BasicEffect>())
            {
                effect.View = _viewMatrix;
                effect.World = _worldMatrix;
                effect.Projection = _projectionMatrix;

                mesh.Draw();
            }
        }*/
    }

    public void DrawGUI(SpriteBatch spriteBatch)
    {
        _canvas.Draw(spriteBatch, Layer.GUILayer);
        Player.Instance.Inventory.Draw(spriteBatch);
        Debug.DrawRectangleBorder(spriteBatch, Input.Mouse.Bounds, Color.Pink, 1);
    }
}