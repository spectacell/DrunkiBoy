﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class TextureManager
    {
        public static Texture2D platform, background1, background2, background3, player, deleteCursor;

        public static void LoadContent(ContentManager content)
        {
            player = content.Load<Texture2D>("player_spriteSheet");
            platform = content.Load<Texture2D>("platform");
            deleteCursor = content.Load<Texture2D>("deleteCursor");
            background1 = content.Load<Texture2D>("bg1");
            background2 = content.Load<Texture2D>("bg2");
            //bakgrund3 = content.Load<Texture2D>("textures/bakgrund3");
        }
    }
}
