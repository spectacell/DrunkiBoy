using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class MenuButton : GameObject
    {
        public MenuButton(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive) :
            base(pos, tex, srcRect, isActive)
        {
           
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(tex, pos, srcRect, Color.White);
        //}
    }
}
