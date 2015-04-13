using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
    {    
    class HamburgareVapen : Bullet
    {
        ParticleEngine particleEngine;
        bool shot;
         public HamburgareVapen(Vector2 pos, Vector2 velocity)
            : base(pos, velocity, Textures.hamburgare, true, 400)    // sätt texturen till hamburgare , 200 för hur långt det åker
        {
            particleEngine = new ParticleEngine(Textures.burgerParticles, pos, Textures.hamburgare, false);
        }
         //public void shot()
         //{
         //    shot = true;
         //    particleEngine.isActive = true;
         //}
        //public void Update(GameTime)
        //{
        //    if (Bullet)
        //}
    }
}
