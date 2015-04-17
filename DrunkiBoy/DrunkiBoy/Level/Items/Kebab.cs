using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class Kebab : GameObject
    {
        public Kebab(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            this.type = "kebab";
        }
    }
}