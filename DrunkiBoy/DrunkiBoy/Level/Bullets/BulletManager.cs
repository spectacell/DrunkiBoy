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

        public static void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet);
        }
        public static void Update(GameTime gameTime, Player player)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Update(gameTime);

                if (!player.animateShooting && !player.weaponThrown)
                {
                    if (bullet.DetectPixelCollision(player)) //Så att player kan plocka upp ammunitionen igen efter att ha kastat iväg den
                    {
                        if (bullet is PizzaWeapon)
                        {
                            player.PickUpAmmo(Player.weaponType.pizza);
                        }
                        else if (bullet is HamburgareVapen)
                        {
                            player.PickUpAmmo(Player.weaponType.burger);
                        }
                        else if (bullet is KebabWeapon)
                        {
                            player.PickUpAmmo(Player.weaponType.kebab);
                        }
                        else if (bullet is BottleWeapon)
                        {
                            player.PickUpAmmo(Player.weaponType.bottle);
                        }
                        else if (bullet is MolotovWeapon)
                        {
                            player.PickUpAmmo(Player.weaponType.bottle);
                        }
                        bullets.Remove(bullet);
                        break;
                    }
                }
                if (bullet.isActive == false)
                {
                    bullets.Remove(bullet);
                    break;
                }
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
        }
    }
}
