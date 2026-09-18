namespace Dreadborne.Scene;

internal sealed class GameScene : IScene
{
    private Player _player;

    public GameScene() { }

    public void Awake(ContentManager content)
    {
        _player = new(Utility.UploadXML("character-prefabs"), "white-man");
    }

    public void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        
    }

    public void Update(GameTime gameTime)
    {
        MainCamera.Follow(_player.Position);
        _player.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _player.Draw(spriteBatch, Layer.PlayerLayer);
    }

    public void DrawGUI(SpriteBatch spriteBatch)
    {

    }
}