using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne.Entity;

internal sealed class DroppedLoot
{
    private readonly List<Item> _droppedItems;
    public IReadOnlyList<Item> DroppedItems => _droppedItems;
    public Item Loots => _droppedItems[0];

    private Rectangle _rectangle;
    public Rectangle Rectangle => _rectangle;

    public DroppedLoot(Item item)
    {
        _droppedItems = [];
        _droppedItems.Add(item);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Item item in _droppedItems)
        {
            item.Draw(spriteBatch, Layer.ItemLayer);
            Debug.DrawRectangleBorder(spriteBatch, _rectangle, Color.LawnGreen, 1);
        }
    }

    public void Drop(Enemy enemy)
    {
        Item item = _droppedItems[0];
        item.SetPosition(enemy.Position);
        _rectangle = new((int)enemy.Position.X - item.Texture.Width / 2, (int)enemy.Position.Y - item.Texture.Height / 2,
            item.Texture.Width, item.Texture.Height);
    }
}