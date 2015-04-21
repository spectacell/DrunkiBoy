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
        public List<Radio> radios = new List<Radio>();
        public static ParticleEngine particleEngine = new ParticleEngine();

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }
        public void AddFlashlight(Flashlight flashlight)
        {
            flashlights.Add(flashlight);
        }

        public void AddRadio(Radio radio)
        {
            radios.Add(radio);
        }

        public void Update(GameTime gameTime, Player player)
        {
            UpdateFlashlights(gameTime, player);
            UpdateRadios(gameTime, player);
        }

        private void UpdateFlashlights(GameTime gameTime, Player player)
        {

            foreach (Flashlight flashlight in flashlights)
            {
              
                flashlight.Update(gameTime);
                if (flashlight.DetectPixelCollision(player))
                {
                    player.LoseHealth(10, flashlight.pos);
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
                        particleEngine.Textures = Textures.pizzaParticles;
                        particleEngine.CreateParticlesInCircleRange(bullet.pos);
                        flashlight.LoseHealth();
                        bullet.isActive = false;
                    }
                    
                }
                
            }
            particleEngine.Update();
        }

        private void UpdateRadios(GameTime gameTime, Player player)
        {

            foreach (Radio radio in radios)
            {
                radio.Update(gameTime);
                if (radio.DetectPixelCollision(player))
                {
                    player.LoseHealth(20, radio.pos);
                    break;
                }

                if (radio.isActive == false)
                {
                    radios.Remove(radio);
                    break;
                }

                foreach (Bullet bullet in BulletManager.bullets)
                {
                    if (radio.DetectPixelCollision(bullet))
                    {
                        particleEngine.Textures = Textures.pizzaParticles;
                        particleEngine.CreateParticlesInCircleRange(bullet.pos);
                        radio.LoseHealth();
                        bullet.isActive = false;
                    }
                }
            }
            particleEngine.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Flashlight flashlight in flashlights)
            {
                flashlight.Draw(spriteBatch);
            }

            foreach (Radio radio in radios)
            {
                radio.Draw(spriteBatch);
            }
            particleEngine.Draw(spriteBatch);
        }
    }
}
