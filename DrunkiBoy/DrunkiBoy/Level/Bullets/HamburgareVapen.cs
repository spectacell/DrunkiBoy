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

         public HamburgareVapen(Vector2 pos, Vector2 velocity)
            : base(pos, velocity, Textures.hamburgare, true, 200)    // sätt texturen till hamburgare , 200 för hur långt det åker
        {

        }               
    }
}
