namespace Dreadborne.UI;

public struct Slot
{
    public const byte SLOT_SIZE = 28;

    private readonly TextureRegion _emptyTextureSlot;
    private readonly TextureRegion _fillTextureSlot;

    public Vector2 Position { get; }
    public Item Item { get; private set; }
    public readonly bool IsEmpty => Item == null;

    public Slot(ContentManager content, Vector2 position)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(content, Utility.UploadXML("gui-prefabs"));
        
        _emptyTextureSlot = atlas.GetRegion("empty-inventory-slot");
        _fillTextureSlot = atlas.GetRegion("equip-inventory-slot");

        Position = position;
        Item = null;
    }

    public void SetItem(Item item)
    {
        Item = item;
        item?.SetPosition(Position);
    }

    public void Clear()
    {
        Item = null;
    }

    public readonly void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        if (Item == null)
            _emptyTextureSlot.Draw(spriteBatch, Position, Color.White,0f,Size.One, SpriteEffects.None, layer.Depth);
        else
            _fillTextureSlot.Draw(spriteBatch, Position, Color.White, 0f, Size.One, SpriteEffects.None, layer.Depth);

        if (!IsEmpty)
        {
            Item?.Draw(spriteBatch, layer);
            if (Item.Staking)
                spriteBatch.DrawString(Canvas.DefaultFont, Item.Amount.ToString(), Position - new Vector2(14, 14), 
                Color.White, 0f, Vector2.Zero, new Vector2(0.8f, 0.8f), SpriteEffects.None, layer.Depth + 0.02f);
        }
    }
}