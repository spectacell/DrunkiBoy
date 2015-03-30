using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy 
{
    class GUIActivePowerUp : GameObject
    {
        private double timeTilNextFrame, frameInterval;
        private int frame, nrFrames, frameWidth;
        public GUIActivePowerUp(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive)
        {
            this.nrFrames = nrFrames;
            this.frameInterval = frameInterval;
            frameWidth = srcRect.Width;
        }

        public virtual void Update(GameTime gameTime)
        {
            timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds; //Animation time counter

            if (timeTilNextFrame <= 0)
            {
                timeTilNextFrame = frameInterval;
                frame++;
                srcRect.X = (frame % nrFrames) * frameWidth;
            }
            if (nrFrames == frame)
            {
                isActive = false;
            }
        }
    }
}
