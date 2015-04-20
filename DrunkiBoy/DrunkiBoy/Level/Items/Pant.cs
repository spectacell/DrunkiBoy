using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    class Pant : GameObject
    {
        ParticleEngine particleEngine;
        bool moving;
        Vector2 targetpos;
       
        public Pant(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive) : base(pos, tex, isActive)
        {
            this.type = "pant";
            particleEngine = new ParticleEngine(Textures.pantParticles, pos, false);
        }
        public void PickUp()
        {
            targetpos.Y = pos.Y - 200;
            moving = true;
            particleEngine.isActive = true;
        }
        public void Update(GameTime gameTime)
        {
            if (moving)
            {
                particleEngine.Update(pos);
                if (pos.Y > targetpos.Y)
                {
                    pos.Y -= 8;
                }
                else
                {
                    isActive = false;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (particleEngine.isActive)
            {
                particleEngine.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
