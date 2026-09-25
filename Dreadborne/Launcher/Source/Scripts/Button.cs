using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Launcher;

internal sealed class Button(ContentManager content, string path, Vector2 position)
{
    private readonly Texture2D _texture = content.Load<Texture2D>(path);
    private Vector2 _position = position;

    private Rectangle _sourceRectangle = new(0, 0, 32, 32);
    public string Text { get; set; } = "Button";

    public Action Click;

    public void Draw(SpriteBatch spriteBatch, float depth, Color buttonColor, Color textColor)
    {
        spriteBatch.Draw(_texture, _position, _sourceRectangle, buttonColor, 0f, new Vector2(_sourceRectangle.Width, _sourceRectangle.Height) * 0.5f, 1f, SpriteEffects.None, depth);

        Vector2 textSize = Application.Font.MeasureString(Text);
        spriteBatch.DrawString(Application.Font, Text, _position, textColor, 0f, textSize * 0.5f, 1f, SpriteEffects.None, depth + 0.005f);
    }
}