namespace Dreadborne;

internal sealed class World
{
    private readonly Tile[] _tiles;
    private readonly int _width, _height;
    
    public int Width => _width;
    public int Height => _height;

    private const byte GRASS_TILES = 12;

    public World(int width, int height)
    {
        this._width = width; this._height = height;
        _tiles = new Tile[_width * _height];

        Generate(out _);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Tile tile in _tiles)
            tile.Draw(spriteBatch, Layer.FloorLayer);
    }

    private void Generate(out Random rand)
    {
        rand = new();

        string path = Utility.UploadXML("floor-tile-prefabs");
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, path);

        string[] grassTilesTexture = new string[GRASS_TILES];

        for (int index = 0; index < grassTilesTexture.Length; index++)
            grassTilesTexture[index] = $"grass-tile-{index}";

        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                _tiles[x + _width * y] = new(atlas.GetRegion(grassTilesTexture[rand.Next(0, GRASS_TILES)]), new(x * Tile.TILE_SIZE, y * Tile.TILE_SIZE));
    }
}