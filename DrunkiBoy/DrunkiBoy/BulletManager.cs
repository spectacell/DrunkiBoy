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
            //här ska vi skriva in de olika bullet som hamburgare, alkohol  osv men vi behöver att skaffa de olika "bullet"klasserna
            // som t.ex. : if ( hamburgare is hamburgareklass)
            bullets.Add(bullet);
            //if (player is )
            //                  { Bullet = new hamburgareklass                 
            //                  }
            //                  else Bullet = new alkoholklass....
                                   
           
        }
        public static void Update(GameTime gameTime)
        {
            // Update loopa genom Bullet listor
            foreach (Bullet bullet in bullets)
            {
                bullet.Update(gameTime);
                if (bullet.isActive == false)
                {
                    bullets.Remove(bullet);
                    break;
                }
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
