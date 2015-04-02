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
        public static List<Bullet> BulletLista = new List<Bullet>();
        public static ParticleEngine particleEngine;

        public static void AddBullet(Enemy target, Player player)
        {
            //här ska vi skriva in de olika bullet som hamburgare, alkohol  osv men vi behöver att skaffa de olika "bullet"klasserna
            // som t.ex. : if ( hamburgare is hamburgareklass)
            Bullet Bullet;
            if (player is )
            //                  { Bullet = new hamburgareklass                 
            //                  }
            //                  else Bullet = new alkoholklass....

            //Bullet Bullet;
           // Bullet = new Bullet(tex, target.pos);     
           //BulletLista.Add(Bullet);
        }
        public static void Update(GameTime gameTime)
        {
            // Update loopa genom Bullet listor
            // remove bullets som träffar enemy , eller går utanför fönster
            particleEngine.Update();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in BulletLista)
            {
                //b.Draw(spriteBatch);          b.Draw går itne att skriva...
            }
            particleEngine.Draw(spriteBatch);
        }
    }
}
