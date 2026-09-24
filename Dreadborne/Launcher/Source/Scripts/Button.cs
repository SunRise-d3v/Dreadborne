using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Launcher;

internal sealed class Button(ContentManager content, string path, Vector2 position)
{
    private readonly Texture2D _texture = content.Load<Texture2D>(path);
    private Vector2 _position = position;
    private string _text = "Button";
    public string Text => _text;

    public Action Click;

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, new Vector2(Application.SCREEN_WIDTH, Application.SCREEN_HEIGHT) * 0.5f, new Rectangle(0, 0, 32, 32), Color.White, 0f, new Vector2(32, 32) * 0.5f, 3f, SpriteEffects.None, 0.2f);
        spriteBatch.DrawString(Application.Font, _text, _position, Color.LightGray);
    }
}