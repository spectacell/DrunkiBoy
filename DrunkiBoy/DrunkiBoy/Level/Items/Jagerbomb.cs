﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Jagerbomb : GameObject
    {
        public Jagerbomb(Vector2 pos, Texture2D tex, bool isActive)
                : base(pos, tex, isActive)
        {
            this.type = "jagerbomb"; 
        }
    }
}