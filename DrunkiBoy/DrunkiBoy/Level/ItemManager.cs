using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class ItemManager
    {
        public List<Platform> platforms = new List<Platform>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<Torch> torches = new List<Torch>();
        public List<Key> keys = new List<Key>();
        public List<Heart> hearts = new List<Heart>();
        public List<Painkiller> painkillers = new List<Painkiller>();
        public List<Teleport> teleports = new List<Teleport>();
        public ItemManager()
        {

        }
        public void AddPlatform(Platform platform)
        {
            platforms.Add(platform);
        }
        public void AddTorch(Torch torch)
        {
            torches.Add(torch);
        }
        public void AddKey(Key key)
        {
            keys.Add(key);
        }
        public void Addheart(Heart heart)
        {
            hearts.Add(heart);
        }
        public void AddPainkiller(Painkiller painkiller)
        {
            painkillers.Add(painkiller);
        }
        public void AddTeleport(Teleport teleport)
        {
            teleports.Add(teleport);
        }
        
        public void Update(GameTime gameTime, Player player)
        {
            UpdatePlatforms(player);
            UpdateTorches(gameTime);
            UpdateKeys(gameTime, player);
            UpdateHeart(gameTime, player);
            UpdatePainkiller(gameTime, player);
            UpdateTeleport(gameTime);
        }

        private void UpdateTeleport(GameTime gameTime)
        {
            foreach (Teleport teleport in teleports)
            {
                teleport.Update(gameTime);
            }
        }

        private void UpdatePainkiller(GameTime gameTime, Player player)
        {
            foreach (Painkiller painkiller in painkillers)
            {
                if (painkiller.DetectPixelCollision(player))
                {
                    painkillers.Remove(painkiller);
                    break;
                }
                painkiller.Update(gameTime);
            }
        }
       
       
        private void UpdateHeart(GameTime gameTime, Player player)
        {
            foreach (Heart heart in hearts)
            {
                if (heart.DetectPixelCollision(player))
                {
                    hearts.Remove(heart);
                    break;
                }
                heart.Update(gameTime);
            }

        }

        private void UpdateKeys(GameTime gameTime, Player player)
        {
            foreach (Key key in keys)
            {
                if (key.DetectPixelCollision(player))
                {
                    keys.Remove(key);
                    break;
                }
                key.Update(gameTime);
            }
        }

        private void UpdateTorches(GameTime gameTime)
        {
            foreach (Torch torchs in torches)
            {
                
                torchs.Update(gameTime);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Platform platform in platforms)
            {
                spriteBatch.Draw(platform.tex, platform.pos, platform.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Torch torchs in torches)
            {
                spriteBatch.Draw(torchs.tex,torchs.pos, torchs.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Key key in keys)
            {
                spriteBatch.Draw(key.tex, key.pos, key.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Heart heart in hearts)
            {
                spriteBatch.Draw(heart.tex, heart.pos, heart.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Painkiller painkiller in painkillers)
            {
                spriteBatch.Draw(painkiller.tex, painkiller.pos, painkiller.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            foreach (Teleport teleport in teleports)
            {
                spriteBatch.Draw(teleport.tex, teleport.pos, teleport.srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
               

        }
        private void UpdatePlatforms(Player player)
        {
            foreach (Platform platform in platforms)
            {
                if (player.movement.Y > 0) //Going down
                {
                    if (player.BottomBoundingBox.Intersects(platform.TopBoundingBox))
                    {
                        player.activePlatform = platform; //Sets the activate platform
                        player.pos.Y = platform.BoundingBox.Top - player.BoundingBox.Height + 1; //+1 to maintain the Intersection
                        player.movement.Y = 0;
                    }
                } 
                
                //foreach (Enemy enemy in enemies)
                //{
                //    if (enemy.activePlatform == null)
                //    {
                //        if (enemy.BoundingBox.Intersects(platform.BoundingBox))// && !enemy.isKilled)
                //        {
                //            enemy.activePlatform = platform; //Sets the activate platform
                //            enemy.pos.Y = platform.BoundingBox.Top - enemy.BoundingBox.Height + 1; //+1 to maintain the Intersection
                //            enemy.isOnGround = true;
                //            //enemy.hasJumped = false;
                //            enemy.movement.Y = 0;
                //        }
                //    }
                //}               
            }
        }
    }
}
