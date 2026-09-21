namespace Dreadborne;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal struct Tile(TextureRegion region, Vector2 position)
{
    public const byte TILE_SIZE = 32;

    private readonly TextureRegion _texture = region;
    private Vector2 _position = position;

    public readonly void Draw(SpriteBatch spriteBatch, Layer layer)
        => _texture.Draw(spriteBatch, _position, Color.White, 0f, Vector2.One, SpriteEffects.None, layer.Depth);
}