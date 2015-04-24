using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class BottleWeapon : Bullet
    {
        public BottleWeapon(Vector2 pos, Vector2 velocity, bool lethal)
            : base(pos, velocity, Textures.bottle, true, 400)
        {
            this.lethal = lethal;
        }   
    }
}
