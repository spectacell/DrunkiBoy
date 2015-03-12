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
        public static Texture2D platform, background1, background2, background3, player, deleteCursor, torchTex, Key, heart, flaska, hamburgare, pizza;

        public static void LoadContent(ContentManager content)
        {
            player = content.Load<Texture2D>("player_spriteSheet");
            platform = content.Load<Texture2D>("platform");
            deleteCursor = content.Load<Texture2D>("deleteCursor");
            background1 = content.Load<Texture2D>("space");
            background2 = content.Load<Texture2D>("bg2");
            torchTex = content.Load<Texture2D>("Torch4");
            Key = content.Load<Texture2D>("Key");
            heart = content.Load<Texture2D>("Extralif2");
            flaska = content.Load<Texture2D>("flaska");
            hamburgare = content.Load<Texture2D>("burgare");
            pizza = content.Load<Texture2D>("Pizza");
            

            //bakgrund3 = content.Load<Texture2D>("textures/bakgrund3");
        }
    }
}
