using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class FireOnGround : AnimatedObject
    {
        public double timeToLive, timeToLiveDefault = 3000;
        public FireOnGround(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            timeToLive = timeToLiveDefault;
        }
        public override void Update(GameTime gameTime)
        {
            timeToLive -= gameTime.ElapsedGameTime.TotalMilliseconds;
            base.Update(gameTime);
            if (timeToLive <= 0)
                isActive = false;
        }
    }
}
