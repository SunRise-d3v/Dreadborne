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

    public static SpriteFont Font;
    public static SpriteFont DefaultFont;

    private const byte MAX_ABILITY = 4;
    private readonly TextureRegion[] _abilities;

    public Canvas(GraphicsDevice graphicsDevice)
    {
        // Font = Game.Content.Load<SpriteFont>("Resources/Fonts/DejaVuSansMono");
        DefaultFont = Game.Content.Load<SpriteFont>("Resources/Fonts/Default");

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
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        spriteBatch.Draw(_pixel, _gameHUDRectangle, _gameHUDRectangle, Color.Black * 0.5f, 0f, Pivot, SpriteEffects.None, layer.Depth);

        byte i = 0;
        byte keys = 1;

        foreach (TextureRegion ability in _abilities)
        {
            ability?.Draw(spriteBatch, new((_rectangleHeight + 150) + Tile.TILE_SIZE * i, Game.SCREEN_HEIGHT - (_rectangleHeight / 2)), Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
            spriteBatch.DrawString(DefaultFont, keys++.ToString(), new((_rectangleHeight + 135) + Tile.TILE_SIZE * i++, Game.SCREEN_HEIGHT - (_rectangleHeight / 2) - Tile.TILE_SIZE / 2), Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, layer.Depth + 0.01f);
        }

        Player.Instance.ManaBar.Draw(spriteBatch, Layer.GUILayer, 3, 5, Color.Gray, Color.Blue);
        Player.Instance.HealthBar.Draw(spriteBatch, Layer.GUILayer, 14, 20, Color.Red, Color.Green);
    }
}