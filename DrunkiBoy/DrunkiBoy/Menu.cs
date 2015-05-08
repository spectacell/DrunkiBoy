using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Menu : GameObject
    {
        public MenuButton startButton;
        public MenuButton optionsButton;
        public MenuButton exitButton;
     
        public Menu(Vector2 pos, Texture2D tex, bool isActive) :
            base (pos, tex, isActive)
        {
            startButton = new MenuButton(new Vector2(100, 100), Textures.startButton, new Rectangle(0, 0, 600, 255), true);
            //startButton.tex = Textures.startButton;
            //optionsButton.tex = Textures.optionsButton;
            //exitButton.tex = Textures.exitButton;

            //startButton = new MenuButton(pos, tex, srcRect, isActive);
            //optionsButton = new MenuButton(pos, tex, srcRect, isActive);
            //exitButton = new MenuButton(pos, tex, srcRect, isActive);
        }

        public void Update(GameTime gameTime)
        {
            CheckButtons();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            startButton.Draw(spriteBatch);
            //optionsButton.Draw(spriteBatch);
            //exitButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void CheckButtons()
        {
            //if (startButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y) || optionsButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y)
            //    || exitButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            //{
            //    //isActive = true; 
            //}
            //else
            //{
            //    //isActive = false; //Om isActive är false så ritas objektet inte ut. Ligger en if-sats i GameObject-klassens Draw() metod
            //}

            if (KeyMouseReader.LeftClick() && startButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                Game1.currentGameState = Game1.gameState.inGame;
            }
            //else if (KeyMouseReader.LeftClick() && isActive == true && optionsButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            //{

            //}
            //else if (KeyMouseReader.LeftClick() && isActive == true && exitButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            //{
                
            //}
        }
        
    }
}
