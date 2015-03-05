namespace DrunkiBoy //http://www.david-gouveia.com/portfolio/2d-camera-with-parallax-scrolling-in-xna/
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public struct ParallaxBackgroundImage
    {
        public Texture2D Texture;
        
        public Vector2 Position;

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Texture != null)
                spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
