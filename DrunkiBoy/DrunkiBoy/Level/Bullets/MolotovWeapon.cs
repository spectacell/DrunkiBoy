using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class MolotovWeapon : Bullet
    {
        public MolotovWeapon(Vector2 pos, Vector2 velocity)
            : base(pos, velocity, Textures.bottle_molotov, true, 400)
        {
            
        } 
    }
}
