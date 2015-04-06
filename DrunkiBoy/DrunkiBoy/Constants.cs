﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Constants
    {
        public static SpriteFont FONT, FONT_BIG;
        public static Color fontColor, fontColor2;
        public static string[] LEVELS = Directory.GetFiles(@"levels\"); //Läser in alla filer som ligger i mappen levels i en array LEVELS

        public static void LoadContent(ContentManager content)
        {
            FONT = content.Load<SpriteFont>("Font");
            FONT_BIG = content.Load<SpriteFont>(@"FontBig");
            fontColor = new Color(214, 214, 214);
            fontColor2 = new Color(200, 132, 35);
        }
    }
}