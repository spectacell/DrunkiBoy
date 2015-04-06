using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    class Pant : GameObject
    {
        public Pant(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive) : base( pos, tex, isActive)
        {
            this.type = "pant";
        }
    }
}
