using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class Key : AnimatedObject
    {
         public Key(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            
            this.type = "Key";            
           
        }
    }
}
