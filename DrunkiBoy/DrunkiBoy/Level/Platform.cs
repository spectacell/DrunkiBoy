using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Platform : GameObject
    {
        public Platform(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive)
            : base(pos, tex, srcRect, isActive)
        {

        }
    }
}
