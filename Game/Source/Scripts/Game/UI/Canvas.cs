namespace Dreadborne.UI;

internal sealed class Canvas
{
    #region Canvas Properties
    private readonly Texture2D _pixel;

    private int _rectangleWidth, _rectangleHeight;
    public int Width => _rectangleWidth;
    public int Height => _rectangleHeight;

    private Rectangle _gameHUDRectangle;

    private Vector2 Pivot => new Vector2(_gameHUDRectangle.Width, _gameHUDRectangle.Height) * 0.5f;
    #endregion

    private const byte MAX_ABILITY = 1;
    private readonly TextureRegion[] _abilities;

    private Player _player;

    public Canvas(GraphicsDevice graphicsDevice, Player player)
    {
        _pixel = new(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _rectangleWidth = 300;
        _rectangleHeight = 135 + 1;

        _gameHUDRectangle = new(Game.SCREEN_WIDTH / 2, Game.SCREEN_HEIGHT - (_rectangleHeight / 2), _rectangleWidth, _rectangleHeight);

        string path = Utility.UploadXML("ability-texture-prefabs");
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, path);

        _abilities = new TextureRegion[MAX_ABILITY];
        for (int index = 0; index < MAX_ABILITY; index++)
            _abilities[index] = atlas.GetRegion("poisoned-arrow");

        this._player = player;
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        spriteBatch.Draw(_pixel, _gameHUDRectangle, _gameHUDRectangle, Color.Black * 0.5f, 0f, Pivot, SpriteEffects.None, layer.Depth);

        foreach (TextureRegion ability in _abilities)
        {
            ability.Draw(spriteBatch, new(Game.SCREEN_WIDTH / 2, Game.SCREEN_HEIGHT - (_rectangleHeight / 2)), Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
        }

        _player.HealthBar.Draw(spriteBatch, Layer.GUILayer, 14, 20, Color.Red, Color.Green);
    }
}