using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class PizzaWeapon : Bullet
    {
        public PizzaWeapon(Vector2 pos, Vector2 velocity)
            : base(pos, velocity, Textures.pizza, true, 400)
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            pos.X += velocity.X;
            pos.Y += (float)Math.Pow(pos.X, 2);
            pos.Y += 0.6f; //gravitation...
            rotation += 0.1f;
            skottRange = skottRange - velocity.Length();

            if (skottRange <= 0)
            {
                isActive = false;
            }
        }
    }
}
