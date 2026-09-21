namespace Dreadborne.Scene;

public enum SceneType : byte { Menu = 0, Game = 1 }
internal interface IScene
{
    void Awake(ContentManager content);
    void Load(GraphicsDevice graphicsDevice, ContentManager content);
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void DrawGUI(SpriteBatch spriteBatch);
}