using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    abstract class AnimatedObject : GameObject
    {
        protected int nrFrames, frame, frameWidth;
        protected double timeTilNextFrame = 0;
        protected const double frameInterval = 80;
        protected Rectangle[] facingSrcRects;
        public int facing = 1; //Vilket håll objektet är vänt åt. Rör man sig åt vänster sätts den till 0 och om man rör sig åt höger så 1.

        public Vector2 movement;
        //public Platform activePlatform; //plattformsklassen är ännu inte skapad
        public bool isOnGround;

        public AnimatedObject(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames) :
            base(pos, tex, srcRect, isActive)
        {
            this.frameWidth = srcRect.Width;
            this.nrFrames = nrFrames;
            //Skapar array med två srcRect som animationen utgår från. En för varje riktning. srcRect för objekt vänt åt höger och 1 för objekt vänt åt vänster.
            facingSrcRects = new Rectangle[2] { srcRect, new Rectangle(nrFrames * srcRect.Width, srcRect.Y, srcRect.Width, srcRect.Height) };
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!(this is Player)) //Player ännu inte skapad. Player sköter sin egen nedräkning i och med att den bara ska räkna när player rör sig. Andra objekt animeras hela tiden.
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds; //Animation time counter

            if (timeTilNextFrame <= 0)
            {
                timeTilNextFrame = frameInterval;
                frame++;
                srcRect.X = facingSrcRects[facing].X + (frame % nrFrames) * frameWidth; //Väljer nästa frame. facingSrcRects[facing] som skapades i constructorn ser till att rätt animation körs. En för vänster och en för höger.
            }
        }

        protected void AddGravity()
        {
            movement.Y += 0.6f;
        }
        /// <summary>
        /// Returns true when object is outside of the visible screen
        /// </summary>
        /// <returns></returns>
        //public bool OutOfBounds()
        //{
        //    if (pos.X < -(Game1.windowWidth / 2) + srcRect.Width || pos.X > Level.currentLevel.levelLength - srcRect.Width || pos.Y > Game1.windowHeight / 2) //Level ännu inte skapad
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
