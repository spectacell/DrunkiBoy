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
        public static SpriteFont FONT, FONT_MEDIUM, FONT_BIG;
        public static Color fontColor, fontColor2;
        public static string[] LEVELS = Directory.GetFiles(@"levels\"); //Läser in alla filer som ligger i mappen levels i en array LEVELS

        public static int health_jagerbomb = 20;
        public static int health_painkiller = 10;
        public static int score_pant = 20;
        public static int score_money = 20;
        public static int health_angryNeightbour = 4;
        public static int health_flashlight = 2;
        public static int health_radio = 3;
        public static int damage_angryNeighbour = 10;
        public static int damage_flashlight = 10;
        public static int damage_radio = 20;
        public static int player_defaultHealth = 200;
        public static int player_defaultLives = 3;

        //Tiden respektive powerup är aktiv i millisekunder
        public static int powerUpTimeVodka = 5000; 
        public static int powerUpTimeRedbullVodka = 5000;

        public static void LoadContent(ContentManager content)
        {
            FONT = content.Load<SpriteFont>("Font");
            FONT_BIG = content.Load<SpriteFont>(@"FontBig");
            FONT_MEDIUM = content.Load<SpriteFont>(@"fontMedium");
            fontColor = new Color(214, 214, 214);
            fontColor2 = new Color(200, 132, 35);
        }
    }
}
