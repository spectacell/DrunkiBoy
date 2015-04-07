namespace DrunkiBoy //http://www.david-gouveia.com/portfolio/2d-camera-with-parallax-scrolling-in-xna/
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class BackgroundImage
    {
        public Texture2D Texture { get; set; }

        public Vector2 pos;

        public BackgroundImage(Vector2 pos, Texture2D tex)
        {
            Texture = tex;
            this.pos = pos;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(Texture != null)
            {
                spriteBatch.Draw(Texture, pos, Color.White); 
            } 
        }
    }
}
