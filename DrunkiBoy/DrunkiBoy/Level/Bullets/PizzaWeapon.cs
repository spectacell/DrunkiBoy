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
        private bool hasTurned;
        float speed = 70;
        public PizzaWeapon(Vector2 pos, Vector2 velocity)
            : base(pos, velocity, Textures.pizza, true, 400)
        {
            this.velocity.Y = -2;
        }
        public override void Update(GameTime gameTime)
        {
            pos += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            pos.Y += 0.3f; //gravitation...
            if (!hasTurned)
            {
                speed -= (float)Math.Pow(4, 2) / 10; //Minskar farten mer och mer tills den vänder
            }
            else
            { 
                speed += (float)Math.Pow(4, 2) / 10; //Ökar farten efter att den vänt
            }
            rotation += 0.1f;
            shotRange = shotRange - velocity.Length();

            if (shotRange <= 0 && !hasTurned)
            {
                velocity *= -1;
                hasTurned = true;
                shotRange = 400;
            }
            if (hasTurned)
            {
                DeactivateIfOutOfBounds(new Rectangle(0, 0, Level.levelWidth, Level.levelHeight));
            }
        }
    }
}
