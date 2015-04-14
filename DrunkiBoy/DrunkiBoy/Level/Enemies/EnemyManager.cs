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
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            UpdateFlashlights(gameTime, player);
        }

        private void UpdateFlashlights(GameTime gameTime, Player player)
        {
            foreach (Flashlight flashlight in flashlights)
            {
                if (flashlight.DetectPixelCollision(player))
                {
                    player.LoseHealth(10);
                    //flashlights.Remove(flashlight);
                    break;
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
