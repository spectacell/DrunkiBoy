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

        protected int levelHeight = 2000, levelWidth = 6000;

        public Level(GraphicsDevice gd, String levelTextFilePath, ContentManager content)
        {
            #region Kamera och parallaxbakgrunder
            this.gd = gd;
            //Allt om kameran här: http://www.david-gouveia.com/portfolio/2d-camera-with-parallax-scrolling-in-xna/
            camera = new Camera(gd.Viewport) { Limits = new Rectangle(0, 0, levelWidth, levelHeight) }; // Rektangeln begränsar kameran.
            layers = new List<BackgroundLayer>
            {
                //Varje lager är en eller flera bakgrunder som rör sig med hastighet specificerat i Vector2
                new BackgroundLayer(camera) { Parallax = new Vector2(0.2f, 1.0f) },
                new BackgroundLayer(camera) { Parallax = new Vector2(0.6f, 1.0f) },
                //new BackgroundLayer(camera) { Parallax = new Vector2(0.4f, 1.0f) }
            };
            // En bakgrund läggs till till varje lager här, går att lägga till flera
            layers[0].AddBackground(new BackgroundImage(new Vector2(0, levelHeight - Textures.background1.Height), Textures.background1));
            layers[1].AddBackground(new BackgroundImage(new Vector2(0, levelHeight - Textures.background2.Height), Textures.background2));

            //layers[2].ListOfBackgrounds.Add(new ParallaxBackgroundImage(new Vector2(0, levelHeight - TextureManager.background3.Height), TextureManager.background3));
            #endregion
            LoadContent(levelTextFilePath);
        }
        
        public virtual void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            itemManager.Update(gameTime, player);

            // Riktar kameran mot spelaren...
            camera.LookAt(player.pos);
        }
        

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (BackgroundLayer layer in layers) //Ritar ut varje lager med alla bakgrunder som finns i respektive
                layer.Draw(spriteBatch);
            Vector2 parallax = new Vector2(1f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            player.Draw(spriteBatch);
            itemManager.Draw(spriteBatch);
            spriteBatch.End();
            
        }

        public void LoadContent(String textFile)
        {
            objects = new List<GameObject>();

            sr = new StreamReader(textFile);

            while (!sr.EndOfStream)
            {
                string[] temp = sr.ReadLine().Split(':'); //Splittar upp textraden i tre delar: type, pos.x, pos.y

                if (temp[0] == "platform")
                {
                    itemManager.AddPlatform(new Platform(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.platform, true));
                    objects.Add(new Platform(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.platform, true)); //För leveleditorn
                }
                else if (temp[0] == "player")
                {
                        player = new Player(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.player, new Rectangle(0,0,95,146), true, 8, 80);
                        objects.Add(player);
                }
                else if (temp[0] == "torch")
                {
                        itemManager.AddTorch(new Torch(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.torchTex, new Rectangle(0, 0, 60, 53), true, 4, 180));
                        objects.Add(new Torch(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.torchTex, new Rectangle(0, 0, 60, 53), true, 4, 180));
                }
                else if (temp[0] == "Key")
                {
                        itemManager.AddKey(new Key(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.Key, new Rectangle(0, 0, 20, 30), true, 3, 250));
                        
                    objects.Add(new Key(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.Key, new Rectangle(0, 0, 20, 30), true, 3, 250));
                }
                else if (temp[0] == "heart")
                {
                    itemManager.Addheart(new Heart(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.heart, new Rectangle(0, 0, 31, 26), true, 2, 250));

                    objects.Add(new Heart(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.heart, new Rectangle(0, 0, 31, 26), true, 2, 250));
                }
                else if (temp[0] == "painkiller")
                {
                    itemManager.AddPainkiller(new Painkiller(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.painkiller, new Rectangle(0, 0, 53, 37), true, 2, 230));

                    objects.Add(new Painkiller(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.painkiller, new Rectangle(0, 0, 53, 37), true, 2, 230));
                }
                else if (temp[0] == "teleport")
                {
                    itemManager.AddTeleport(new Teleport(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.teleport, new Rectangle(0, 0, 200, 267), true, 2, 100));

                    objects.Add(new Teleport(new Vector2(Convert.ToInt16(temp[1]), Convert.ToInt16(temp[2])), Textures.teleport, new Rectangle(0, 0, 200, 267), true, 2, 100));
                }
            }
            sr.Close();
        }

    }
    
}
