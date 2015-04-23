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
        public PizzaWeapon(Vector2 pos, Vector2 velocity)
            : base(pos, velocity, Textures.pizza, true, 450)
        {
            this.velocity.Y = -2; //Så att går lite snett uppåt
        }
        public override void Update(GameTime gameTime)
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
