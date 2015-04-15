using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class EnemyManager
    {
        public List<Enemy> enemies = new List<Enemy>();
        public List<Flashlight> flashlights = new List<Flashlight>();

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }
        public void AddFlashlight(Flashlight flashlight)
        {
            flashlights.Add(flashlight);
        }

        public void Update(GameTime gameTime, Player player)
        {
            UpdateFlashlights(gameTime, player);
        }

        private void UpdateFlashlights(GameTime gameTime, Player player)
        {

            foreach (Flashlight flashlight in flashlights)
            {
                flashlight.Update(gameTime);
                if (flashlight.DetectPixelCollision(player))
                {
                    player.LoseHealth(10, flashlight.pos);
                    //flashlights.Remove(flashlight);
                    break;
                }

                if (flashlight.isActive == false)
                {
                    flashlights.Remove(flashlight);
                    break;
                }

                foreach (Bullet bullet in BulletManager.bullets)
                {
                    if (flashlight.DetectPixelCollision(bullet))
                    {
                        flashlight.LoseHealth();
                        bullet.isActive = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Flashlight flashlight in flashlights)
            {
                flashlight.Draw(spriteBatch);
            }
        }
    }
}
