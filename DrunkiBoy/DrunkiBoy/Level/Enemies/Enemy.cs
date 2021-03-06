﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DrunkiBoy
{
    abstract class Enemy : AnimatedObject
    {
        protected float health;

        public Enemy(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {

        }
        public void LoseHealth(float amountToLoose)
        {
            if (health > 0)
            {
                health -= amountToLoose;
            }

            if (health <= 0)
            {
                isActive = false;
            }
        }
    }
}
