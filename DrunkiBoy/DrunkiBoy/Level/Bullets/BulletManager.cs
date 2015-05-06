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
        public static List<Bullet> bullets = new List<Bullet>();
        public static ParticleEngine particleEngine = new ParticleEngine();

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

                if (!player.animateShooting && !player.weaponThrown && bullet is PizzaWeapon) 
                {
                    if (bullet.DetectPixelCollision(player)) //Så att player kan plocka upp pizzan igen efter att ha kastat iväg den
                    {
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
            }

            //remove bullets som träffar enemy , eller går utanför fönster

        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
            {

                b.Draw(spriteBatch);
            }
        }

        //public void AddBurgerAmmo
        //{

        //}
    }
}
