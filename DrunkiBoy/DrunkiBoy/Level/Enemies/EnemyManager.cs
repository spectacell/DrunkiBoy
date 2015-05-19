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
        public List<AngryNeighbour> angryNeighbours = new List<AngryNeighbour>();

        public static ParticleEngine particleEngine = new ParticleEngine();

        public void Update(GameTime gameTime, Player player)
        {
            UpdateFlashlights(gameTime, player);
            UpdateRadios(gameTime, player);
            UpdateAngryNeighbours(gameTime, player);
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
            foreach (AngryNeighbour angryNeighbour in angryNeighbours)
            {
                angryNeighbour.Draw(spriteBatch);
            }
            particleEngine.Draw(spriteBatch);
        }

        private void UpdateAngryNeighbours(GameTime gameTime, Player player)
        {
            foreach (AngryNeighbour an in angryNeighbours)
            {
                an.Update(gameTime);
                if (!player.spawning && an.DetectPixelCollision(player))
                {
                    player.LoseHealth(Constants.damage_angryNeighbour, an.pos, an.srcRect.Width);
                    if (player.facing == an.facing)
                        an.ChangeDirection();
                    break;
                }

                if (an.isActive == false)
                {
                    angryNeighbours.Remove(an);
                    break;
                }

                foreach (Bullet bullet in BulletManager.bullets)
                {
                    if (an.DetectPixelCollision(bullet))
                    {
                        Sound.hitWithShot.Play();
                        GenerateParticleEngine(bullet);
                        if (bullet.lethal)
                            an.LoseHealth(1);
                        if (player.facing == an.facing)
                            an.ChangeDirection();
                        bullet.isActive = false;
                        an.BlinkHealthBar();
                    }  
                }
            }
            particleEngine.Update();
        }
        private void UpdateFlashlights(GameTime gameTime, Player player)
        {
            foreach (Flashlight flashlight in flashlights)
            {
                flashlight.Update(gameTime);
                if (flashlight.DetectPixelCollision(player))
                {
                    player.LoseHealth(Constants.damage_flashlight, flashlight.pos, flashlight.srcRect.Width);
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
                        Sound.hitWithShot.Play();
                        GenerateParticleEngine(bullet);
                        if (bullet.lethal)
                            flashlight.LoseHealth(1);
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
                    player.LoseHealth(Constants.damage_radio, radio.pos, radio.srcRect.Width);
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
                        Sound.hitWithShot.Play();
                        GenerateParticleEngine(bullet);
                        if (bullet.lethal)
                            radio.LoseHealth(1);
                        bullet.isActive = false;
                    }
                }
            }
            particleEngine.Update();
        }

        private static void GenerateParticleEngine(Bullet bullet)
        {
            if (bullet is PizzaWeapon)
            {
                particleEngine.Textures = Textures.pizzaParticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is HamburgareVapen)
            {
                particleEngine.Textures = Textures.burgerParticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is BottleWeapon)
            {
                particleEngine.Textures = Textures.bottleparticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is MolotovWeapon)
            {
                particleEngine.Textures = Textures.bottleparticles;
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            if (bullet is KebabWeapon)
            {
                particleEngine.Textures = Textures.kebabParticles; 
                particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
        }
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

        public void AddAngryNeighbour(AngryNeighbour an)
        {
            angryNeighbours.Add(an);
        }
    }
}
