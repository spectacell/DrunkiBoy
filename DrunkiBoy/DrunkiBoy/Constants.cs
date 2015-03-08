using Microsoft.Xna.Framework;
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
        public static char platformCharSymbol = 'P';
        public static char playerCharSymbol = 'Y';
        public static Rectangle PLATFORM_SRC_RECT, PLAYER_SRC_RECT, ARROW_SRC_RECT;

        public static string[] LEVELS = Directory.GetFiles(@"levels\"); //Läser in alla filer som ligger i mappen levels i en array LEVELS

        public static void LoadContent(ContentManager content)
        {
            FONT = content.Load<SpriteFont>("Font");
            FONT_BIG = content.Load<SpriteFont>(@"FontBig");
        }
    }
}
