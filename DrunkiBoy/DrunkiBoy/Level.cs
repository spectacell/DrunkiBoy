using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DrunkiBoy
{
    class Level
    {
        Player player;
        GraphicsDevice gd;
        
        Camera camera;
        List<BackgroundLayer> layers;

        public Level(GraphicsDevice gd)
        {
            #region Kamera och parallaxbakgrunder
            this.gd = gd;
            //Allt om kameran här: http://www.david-gouveia.com/portfolio/2d-camera-with-parallax-scrolling-in-xna/
            // Rektangeln begränsar kameran. Just nu börjar vid 0 och slutar vid 3200 med höjd 600
            camera = new Camera(gd.Viewport) { Limits = new Rectangle(0, 0, 3200, 600) };
            layers = new List<BackgroundLayer>
            {
                //Varje lager är en eller flera bakgrunder som rör sig med hastighet specificerat i Vector2
                new BackgroundLayer(camera) { Parallax = new Vector2(0.0f, 1.0f) },
                new BackgroundLayer(camera) { Parallax = new Vector2(0.1f, 1.0f) },
                new BackgroundLayer(camera) { Parallax = new Vector2(0.2f, 1.0f) }
            };
            // En bakgrund läggs till till varje lager här, går att lägga till flera
            layers[0].Backgrounds.Add(new ParallaxBackgroundImage { Texture = TextureManager.bakgrund1 });
            layers[1].Backgrounds.Add(new ParallaxBackgroundImage { Texture = TextureManager.bakgrund2 });
            layers[2].Backgrounds.Add(new ParallaxBackgroundImage { Texture = TextureManager.bakgrund3 });
#endregion
        }
        
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            // Riktar kameran mot spelaren...
            camera.LookAt(player.pos);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BackgroundLayer layer in layers) //Ritar ut varje lager med alla bakgrunder som finns i respektive
                layer.Draw(spriteBatch);
        }
    }
    
}
