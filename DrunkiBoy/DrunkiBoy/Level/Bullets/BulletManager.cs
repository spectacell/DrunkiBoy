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
                if (!player.animateShooting && bullet is PizzaWeapon) 
                {
                    if (bullet.DetectPixelCollision(player))
                    {
                        bullets.Remove(bullet);
                        player.PickUpWeapon(Player.weaponType.pizza);
                        break;
                    }
                }
                bullet.OutOfRange();
                if (bullet.isActive == false)
                {
                    bullets.Remove(bullet);
                    break;
                }
                //particleEngine.CreateParticlesInCircleRange(bullet.pos);
            }
            
           
            // remove bullets som träffar enemy , eller går utanför fönster
            //particleEngine.Update();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);          
            }
            //particleEngine.Draw(spriteBatch);
        }
    }
}
