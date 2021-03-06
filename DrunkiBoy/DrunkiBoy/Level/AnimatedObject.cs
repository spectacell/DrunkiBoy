﻿using System;
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
        protected double frameInterval;
        protected Rectangle[] facingSrcRects;
        public int facing = 1; //Vilket håll objektet är vänt åt. Rör man sig åt vänster sätts den till 0 och om man rör sig åt höger så 1.

        public Vector2 movement;
        public Platform activePlatform;

        public AnimatedObject(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval) :
            base(pos, tex, srcRect, isActive)
        {
            this.frameInterval = frameInterval;
            this.frameWidth = srcRect.Width;
            this.nrFrames = nrFrames;
            timeTilNextFrame = frameInterval;
            //Skapar array med två srcRect som animationen utgår från. En för varje riktning. 0 för objekt vänt åt höger och 1 för objekt vänt åt vänster.
            facingSrcRects = new Rectangle[2] { new Rectangle(nrFrames * srcRect.Width, srcRect.Y, srcRect.Width, srcRect.Height), srcRect };
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!(this is Player)) //Player sköter sin egen nedräkning i och med att den bara ska räkna när player rör sig. Andra objekt animeras hela tiden.
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds; //Animation time counter

            if (timeTilNextFrame <= 0)
            {
                timeTilNextFrame = frameInterval;
                frame++;
                srcRect.X = facingSrcRects[facing].X + (frame % nrFrames) * frameWidth; //Väljer nästa frame. facingSrcRects[facing] som skapades i constructorn ser till att rätt animation körs. En för vänster och en för höger.
            }
            
        }

        protected void AddGravity(float amount)
        {
            movement.Y += amount;
        }
    }
}
