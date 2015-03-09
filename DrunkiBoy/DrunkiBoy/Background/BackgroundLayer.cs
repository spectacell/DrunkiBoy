namespace DrunkiBoy //http://www.david-gouveia.com/portfolio/2d-camera-with-parallax-scrolling-in-xna/
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    public class BackgroundLayer
    {
        private readonly Camera camera;
        public Vector2 Parallax { get; set; }
        public List<ParallaxBackgroundImage> Backgrounds { get; private set; }
        public BackgroundLayer(Camera camera)
        {
            this.camera = camera;
            Backgrounds = new List<ParallaxBackgroundImage>();
        }
        public void Draw(SpriteBatch spriteBatch)
	    {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(Parallax));
    	    
            foreach(ParallaxBackgroundImage bg in Backgrounds)
		        bg.Draw(spriteBatch);
            
            spriteBatch.End();
	    }

        //Ev. ha de här två i LevelEditorn...lite osäker
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
