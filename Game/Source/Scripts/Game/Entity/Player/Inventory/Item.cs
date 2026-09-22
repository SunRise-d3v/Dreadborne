namespace Dreadborne;

public sealed class Item
{
    private readonly TextureRegion _texture;
    public TextureRegion Texture => _texture;

    private Vector2 _position;

    public string ItemId { get; }
    public byte Amount { get; set; }
    public byte MaxAmount { get; }
    public bool Staking { get; }
    public bool Equiped { get; set; }

    private readonly Vector2 _scaleInSlot;

    public Item(string path, byte amount, byte maxAmount = 99, bool staking = true)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, Utility.UploadXML("item-prefabs"));
        _texture = atlas.GetRegion(path);

        ItemId = path;
        this.Amount = amount;
        this.MaxAmount = maxAmount;

        this.Staking = staking;
        if (!Staking)
            MaxAmount = 1;

        _scaleInSlot = new Vector2(0.92f, 0.92f);
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        _texture.Draw(spriteBatch, _position, Color.White, 0f, _scaleInSlot, SpriteEffects.None, layer.Depth + 0.01f);
    }

    private Vector2 CentrePivot()
    {
        return new Vector2(_texture.Width, _texture.Height) * 0.5f;
    }
}