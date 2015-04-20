using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    class Painkiller : AnimatedObject
    {
        ParticleEngine particleEngine;
        bool moving;
        Vector2 targetpos;
         public Painkiller(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {            
            this.type = "painkiller";
            particleEngine = new ParticleEngine(Textures.painkillerParticles, pos, false);
           
        }
         public void PickUp()
         {
             targetpos.Y = pos.Y - 200;
             moving = true;
             particleEngine.isActive = true;
         }
         public override void Update(GameTime gameTime)
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

             base.Update(gameTime);
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
