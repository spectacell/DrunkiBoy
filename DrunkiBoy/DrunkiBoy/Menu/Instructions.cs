using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
namespace DrunkiBoy
{
    class Instructions : GameObject
    {
        private string strGoBack = "Click here to go back";
        private Text textGoBack;
        public Instructions(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            textGoBack = new Text(Constants.FONT, strGoBack, new Vector2(10, 740));
        }
        public void Update()
        {
            if (textGoBack.IsClicked() || KeyMouseReader.KeyPressed(Keys.Escape))
            {
                Menu.showInstructions = false;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            textGoBack.DrawClickableText(spriteBatch, Color.Goldenrod);
        }
    }
}