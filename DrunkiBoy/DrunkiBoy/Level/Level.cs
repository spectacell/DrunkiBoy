using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace DrunkiBoy
{
    class Level
    {
        protected Player player;
        protected GraphicsDevice gd;
        
        protected Camera camera;
        protected List<BackgroundLayer> layers;

        protected StreamReader sr;
        protected List<GameObject> objects; //For the Level editor

        protected ItemManager itemManager = new ItemManager();
        public Level(GraphicsDevice gd, String levelTextFilePath, ContentManager content)
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
            layers[0].Backgrounds.Add(new ParallaxBackgroundImage { Texture = TextureManager.background1 });
            layers[1].Backgrounds.Add(new ParallaxBackgroundImage { Texture = TextureManager.background2 });
            layers[2].Backgrounds.Add(new ParallaxBackgroundImage { Texture = TextureManager.background3 });
            #endregion
            LoadContent(levelTextFilePath);
            player = new Player(new Vector2(100,100), TextureManager.player, new Rectangle(0,0,280,220), true, 1);
        }
        
        public virtual void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            itemManager.Update(player, gameTime);

            // Riktar kameran mot spelaren...
            camera.LookAt(player.pos);
        }
        

        public virtual void Draw(SpriteBatch spriteBatch)
        {        
            Vector2 parallax = new Vector2(1f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            player.Draw(spriteBatch);
            spriteBatch.End();
            foreach (BackgroundLayer layer in layers) //Ritar ut varje lager med alla bakgrunder som finns i respektive
                layer.Draw(spriteBatch);
        }

        public void LoadContent(String textFile)
        {
            objects = new List<GameObject>();

            sr = new StreamReader(textFile);

            while (!sr.EndOfStream)
            {
                string[] temp = sr.ReadLine().Split(':');

                if (temp[0] == "P")
                {
                    itemManager.AddPlatform(new Platform(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), TextureManager.platform, true));

                    if (levelEditor)
                        objects.Add(new Platform(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), TextureManager.platform, true));
                    else
                        itemManager.AddPlatform(new Platform(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), TextureManager.platform, true));
                }
                //else if (temp[0] == "Y")
                //{
                //    if (levelEditor)
                //        objects.Add(new Player(Constants.playerCharSymbol, new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Constants.PLAYER_SRC_RECT));
                //    else
                //        player = new Player(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Constants.PLAYER_SRC_RECT, 4);
                //}

            }
            sr.Close();
        }

    }
    
}
