using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    // kan vara onödigt klass
    class HamburgareVapen : Bullet
    {
        bool shot;
         public HamburgareVapen(Vector2 pos, Vector2 velocity)
            : base(pos, velocity, Textures.hamburgare, true, 400)    // sätt texturen till hamburgare , 200 för hur långt det åker
        {

        }
        //public void Update(GameTime)
        //{
        //    if (Bullet)
        //}
    }
}
