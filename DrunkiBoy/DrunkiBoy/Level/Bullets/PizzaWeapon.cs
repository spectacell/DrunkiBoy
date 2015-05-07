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
        float speed = 70;
        bool thrownInAir;
        public PizzaWeapon(Vector2 pos, Vector2 velocity, bool thrownInAir, bool lethal)
            : base(pos, velocity, Textures.pizza, true, 400)
        {
            this.thrownInAir = thrownInAir;
            this.lethal = lethal;
        }
        public override void Update(GameTime gameTime)
        {
            if (thrownInAir)
            {
                base.Update(gameTime);
            }
            else 
            { 
                pos += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                pos.Y += 0.3f; //gravitation...
                speed -= (float)Math.Pow(4, 2) / 10; //Minskar farten mer och mer tills den vänder

                rotation += 0.1f;
                shotRange = shotRange - velocity.Length();
                DeactivateIfOutOfBounds(new Rectangle(0, 0, Level.levelWidth, Level.levelHeight));
            }
        }
    }
}
