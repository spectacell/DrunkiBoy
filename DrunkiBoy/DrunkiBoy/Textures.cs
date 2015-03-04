using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Textures
    {
        public static Texture2D aTexture;

        public static void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            aTexture = content.Load<Texture2D>("textures/textureName");
        }
    }
}
