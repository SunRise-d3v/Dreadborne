using Debug = Dreadborne.DeveloperTools.Debug;

namespace Dreadborne.UI;

internal sealed class Inventory
{
    private readonly Texture2D _pixel;
    private Rectangle _backgroundCanvas;

    private Rectangle _dragNDropField;
    public Rectangle DragNDropField => _dragNDropField;

    #region Const
    private const byte INVENTORY_CAPACITY_X = 10;
    private const byte INVENTORY_CAPACITY_Y = 7;
    private const float OFFSET = 0.8f;
    #endregion

    private readonly Slot[] _slots;
    internal Vector2 _position;

    private const int PADDING = 12;
    private const int TITLE_HEIGHT = 28;

    internal bool IsOpen { get; set; }

    public Inventory(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _pixel = new(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _slots = new Slot[INVENTORY_CAPACITY_X * INVENTORY_CAPACITY_Y];

        float gridWidth = INVENTORY_CAPACITY_X * Slot.SLOT_SIZE + (INVENTORY_CAPACITY_X - 1) * OFFSET;
        float gridHeight = INVENTORY_CAPACITY_Y * Slot.SLOT_SIZE + (INVENTORY_CAPACITY_Y - 1) * OFFSET;

        float panelWidth = gridWidth + PADDING * 2;
        float panelHeight = gridHeight + PADDING * 2 + TITLE_HEIGHT;

        _backgroundCanvas = new(
            (int)(Game.SCREEN_WIDTH - panelWidth) / 2, (int)(Game.SCREEN_HEIGHT - panelHeight) / 2,
            (int)panelWidth, (int)panelHeight);

        for (int indexY = 0; indexY < INVENTORY_CAPACITY_Y; indexY++)
            for (int indexX = 0; indexX < INVENTORY_CAPACITY_X; indexX++)
            {
                float x = _backgroundCanvas.X + PADDING + (Slot.SLOT_SIZE + OFFSET) * indexX;
                float y = _backgroundCanvas.Y + PADDING + TITLE_HEIGHT + (Slot.SLOT_SIZE + OFFSET) * indexY;

                _position = new(x, y);
                _slots[indexX + INVENTORY_CAPACITY_X * indexY] = new(content, _position);
            }

        _dragNDropField = new(
            (int)(Game.SCREEN_WIDTH - panelWidth) / 2, (int)(Game.SCREEN_HEIGHT - panelHeight) / 2,
            (int)panelWidth, TITLE_HEIGHT);

        IsOpen = false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsOpen)
            return;
        else
        {
            spriteBatch.Draw(_pixel, _backgroundCanvas, null, Color.Black,
                0f, Vector2.Zero, SpriteEffects.None, Layer.UILayer.Depth - 0.01f);

            string title = "Inventory";
            Vector2 textSize = Canvas.DefaultFont.MeasureString(title);
            Vector2 titlePosition = new(_backgroundCanvas.X + _backgroundCanvas.Width / 2f, _backgroundCanvas.Y + PADDING / 2f);

            spriteBatch.DrawString(Canvas.DefaultFont, title, titlePosition, Color.White,
                0f, new Vector2(textSize.X / 2f, 0f), 1f, SpriteEffects.None, Layer.UILayer.Depth);

            foreach (Slot slot in _slots)
                slot.Draw(spriteBatch, Layer.UILayer);
        }

        Debug.DrawRectangleBorder(spriteBatch, _dragNDropField, Color.Green, 1);
    }

    #region Inventory Logic
    public bool AddItem(Item item)
    {
        for (int y = 0; y < INVENTORY_CAPACITY_Y; y++)
            for (int x = 0; x < INVENTORY_CAPACITY_X; x++)
            {
                ref Slot slot = ref _slots[x + INVENTORY_CAPACITY_X * y];

                if (slot.IsEmpty || slot.Item.ItemId != item.ItemId)
                    continue;

                if (slot.Item.Amount >= slot.Item.MaxAmount)
                    continue;

                int spaceLeft = slot.Item.MaxAmount - slot.Item.Amount;
                int toMove = Math.Min(spaceLeft, item.Amount);

                slot.Item.Amount += (byte)toMove;
                item.Amount -= (byte)toMove;

                if (item.Amount == 0)
                    return true;
            }

        for (int y = 0; y < INVENTORY_CAPACITY_Y; y++)
            for (int x = 0; x < INVENTORY_CAPACITY_X; x++)
            {
                ref Slot slot = ref _slots[x + INVENTORY_CAPACITY_X * y];

                if (!slot.IsEmpty)
                    continue;

                slot.SetItem(item);
                return true;
            }

        return false;
    }

    public bool RemoveItem(string itemId, byte amount)
    {
        byte remaining = amount;

        for (int y = 0; y < INVENTORY_CAPACITY_Y; y++)
            for (int x = 0; x < INVENTORY_CAPACITY_X; x++)
            {
                ref Slot slot = ref _slots[x + INVENTORY_CAPACITY_X * y];

                if (slot.IsEmpty || slot.Item.ItemId != itemId)
                    continue;

                byte toRemove = (byte)Math.Min(remaining, slot.Item.Amount);
                slot.Item.Amount -= toRemove;
                remaining -= toRemove;

                if (slot.Item.Amount == 0)
                    slot.Clear();

                if (remaining == 0)
                    return true;
            }

        return remaining == 0;
    }
    #endregion
}