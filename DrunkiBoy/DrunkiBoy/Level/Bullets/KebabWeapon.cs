using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class KebabWeapon : Bullet
    {
        public KebabWeapon(Vector2 pos, Vector2 velocity, bool lethal)
            : base(pos, velocity, Textures.kebab, true, 400)
        {
            this.lethal = lethal;
        }
    }
}
