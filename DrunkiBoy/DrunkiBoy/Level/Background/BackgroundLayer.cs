namespace DrunkiBoy //http://www.david-gouveia.com/portfolio/2d-camera-with-parallax-scrolling-in-xna/
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    public class BackgroundLayer
    {
        private readonly Camera camera;
        public Vector2 Parallax { get; set; }
        public List<BackgroundImage> ListOfBackgrounds { get; private set; }
        public BackgroundLayer(Camera camera)
        {
            this.camera = camera;
            ListOfBackgrounds = new List<BackgroundImage>();
        }
        public void Draw(SpriteBatch spriteBatch)
	    {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(Parallax));

            foreach (BackgroundImage bg in ListOfBackgrounds)
            {
                bg.Draw(spriteBatch);
            }
            spriteBatch.End();
	    }
        /// <summary>
        /// Adds a background to the List of backgrounds in the layer. Also creates 3 additional backgrounds surrounding the first one.
        /// </summary>
        /// <param name="image">The image to add</param>
        public void AddBackground(BackgroundImage image)
        {
            ListOfBackgrounds.Add(image);
            ListOfBackgrounds.Add(new BackgroundImage(new Vector2(image.pos.X + image.Texture.Width, image.pos.Y), image.Texture));
            ListOfBackgrounds.Add(new BackgroundImage(new Vector2(image.pos.X, image.pos.Y - image.Texture.Height), image.Texture));
            ListOfBackgrounds.Add(new BackgroundImage(new Vector2(image.pos.X + image.Texture.Width, image.pos.Y - image.Texture.Height), image.Texture)); 
            //Lägger till tre bakgrunder av samma, runt om den första. Lär gå att lösa snyggare genom att få bakgrunderna att upprepas automatiskt
        }
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, camera.GetViewMatrix(Parallax));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(camera.GetViewMatrix(Parallax)));
        }
    }
}
