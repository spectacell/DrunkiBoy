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
        public Enemy(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames)
            : base(pos, tex, srcRect, isActive, nrFrames)
        {

        }
    }
}
