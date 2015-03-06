using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class TextureManager
    {
        public static Texture2D aTexture, bakgrund1, bakgrund2, bakgrund3, player;

        public static void LoadContent(ContentManager content)
        {
            player = content.Load<Texture2D>("player_spriteSheet");
            //bakgrund1 = content.Load<Texture2D>("textures/bakgrund1");
            //bakgrund2 = content.Load<Texture2D>("textures/bakgrund2");
            //bakgrund3 = content.Load<Texture2D>("textures/bakgrund3");
        }
    }
}
