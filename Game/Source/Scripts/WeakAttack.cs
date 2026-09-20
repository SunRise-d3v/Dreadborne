namespace Dreadborne;

internal sealed class WeakAttack
{
    private TextureRegion _texture;
    private Vector2 _position;

    public WeakAttack()
    {
        string path = Utility.UploadXML("projectile-prefabs");
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, path);

        _texture = atlas.GetRegion("sword");
    }

    public void Attack()
    {
        //Projectile.NewProjectile();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _texture.Draw(spriteBatch, _position, Color.White, 0f, Vector2.One, SpriteEffects.None, Layer.EntityLayer.Depth);
    }
}