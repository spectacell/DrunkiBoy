using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DrunkiBoy
{
    class Instructions : GameObject
    {
        private string strGoBack = "Click here to go back to the menu";
        private Text textGoBack;
        public Instructions(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            textGoBack = new Text(Constants.FONT, strGoBack, new Vector2(20, 650));
        }
        public void Update()
        {
            if (textGoBack.IsClicked())
            {
                Game1.currentGameState = Game1.gameState.menu;
            }
        }
    }
}