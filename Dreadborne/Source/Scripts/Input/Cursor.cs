namespace Dreadborne;

public class Cursor
{
    private readonly TextureRegion _texture;
    private Vector2 _position;

    public Cursor(string pathFile, string field)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, pathFile);
        this._texture = atlas.GetRegion(field);
    }

    public void Update() => _position = Input.Mouse.Position.ToVector2();

    public void Draw(SpriteBatch spriteBatch, Layer layer)
        => _texture.Draw(spriteBatch, _position, Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
}