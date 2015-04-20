using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace DrunkiBoy
{
    static class BulletManager
    {
        public static Texture2D tex;
        public static List<Bullet> bullets = new List<Bullet>();
        public static ParticleEngine particleEngine;
        public static Vector2 pos;
        

        

        public static void AddBullet(Bullet bullet)
        {
            
            bullets.Add(bullet);
        }
        public static void Update(GameTime gameTime, Player player)
        {
            
            // Update loopa genom Bullet listor
            foreach (Bullet bullet in bullets)
            {
                
                bullet.Update(gameTime);
                particleEngine = new ParticleEngine(Textures.pizzaParticles, pos, true);
                if (!player.animateShooting && bullet is PizzaWeapon) 
                {
                    
                    
                    if (bullet.DetectPixelCollision(player))
                    {                        
                        particleEngine.CreateParticlesInCircleRange(bullet.pos);
                         
                        bullets.Remove(bullet);                        
                        player.PickUpWeapon(Player.weaponType.pizza);
                        
                        break;
                    }
                    
                }
                
                if (bullet.isActive == false)
                {                                        
                    bullets.Remove(bullet);
                    break;
                }
                particleEngine.Update(pos);
            }

            
            //remove bullets som träffar enemy , eller går utanför fönster
            //particleEngine.CreateParticlesInCircleRange(bullet.pos);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
            {
                if (particleEngine.isActive)
                {
                    particleEngine.Draw(spriteBatch);
                }
                b.Draw(spriteBatch);
            }
            
            
            
        }
    }
}
