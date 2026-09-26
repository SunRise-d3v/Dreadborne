using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Launcher;

internal sealed class Button(ContentManager content, string path, Vector2 position)
{
    private readonly Texture2D _texture = content.Load<Texture2D>(path);
    private XnaVector2 _position = position;

    private Rectangle _boundingBox = Rectangle.Empty;
    private Rectangle _sourceRectangle = new(0, 0, 80, 32);
    public string Text { get; set; } = "Button";

    public bool Enable { get; set; } = false;

    public Action Click;

    public void Draw(SpriteBatch spriteBatch, float depth, XnaColor buttonColor, XnaColor textColor, float textScale = 1f)
    {
        _boundingBox = new(
            (int)(_position.X - _sourceRectangle.Width * 0.5f),
            (int)(_position.Y - _sourceRectangle.Height * 0.5f),
            _sourceRectangle.Width,
            _sourceRectangle.Height);

        buttonColor = Enable ? buttonColor : XnaColor.Gray;

        spriteBatch.Draw(_texture, _position, _sourceRectangle, buttonColor, 0f, new XnaVector2(_sourceRectangle.Width, _sourceRectangle.Height) * 0.5f, 1f, SpriteEffects.None, depth);

        XnaVector2 textSize = Application.Font.MeasureString(Text);
        spriteBatch.DrawString(Application.Font, Text, _position, textColor, 0f, textSize * 0.5f, textScale, SpriteEffects.None, depth + 0.005f);
    }

    public bool Contains(Point point)
    {
        Rectangle bounds = new(
            (int)(_position.X - _sourceRectangle.Width * 0.5f),
            (int)(_position.Y - _sourceRectangle.Height * 0.5f),
            _sourceRectangle.Width,
            _sourceRectangle.Height);

        return bounds.Contains(point);
    }

    public void OnClick()
    {
        if (Enable)
            Click?.Invoke();
    }
}