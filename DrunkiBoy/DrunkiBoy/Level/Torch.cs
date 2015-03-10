using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Torch : AnimatedObject
    {
       
        
        public Torch(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            
            this.type = "torch";            
           
        }
        //Kan ta bort Draw() härifrån sen när Torch är implementerad, eftersom den använder Draw i AnimatedObject klassen
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(tex, pos, Color.White);
            base.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
