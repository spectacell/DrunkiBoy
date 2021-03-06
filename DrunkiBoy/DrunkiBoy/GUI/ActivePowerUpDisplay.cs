﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy 
{
    class ActivePowerUpDisplay : GameObject
    {
        private double timeTilNextFrame, frameInterval;
        private int frame, nrFrames, frameWidth;
        private Texture2D powerUpTexture;
        private Vector2 vodkaPos;
        public ActivePowerUpDisplay(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval, int powerUpToDisplay)
            : base(pos, tex, srcRect, isActive)
        {
            this.nrFrames = nrFrames;
            this.frameInterval = frameInterval;
            frameWidth = srcRect.Width;
            timeTilNextFrame = frameInterval;
            vodkaPos = pos + pos / 12;

            switch (powerUpToDisplay)
            {
                case 1: //Odödlighet
                    powerUpTexture = Textures.vodka_gui;
                    break;
                case 2: //Flygförmåga
                    powerUpTexture = Textures.redbullVodka_gui;
                    break;
            }
        }

        public void Update(GameTime gameTime)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.Draw(powerUpTexture, pos, Color.White);
            }
            base.Draw(spriteBatch);
        }
    }
}
