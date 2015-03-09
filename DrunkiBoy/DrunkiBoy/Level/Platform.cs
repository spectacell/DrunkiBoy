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
        public Platform(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            this.type = "platform"; //Bokstav eller namn som identifierar objektet i textfilen som läser in banan
        }
    }
}
