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
     
        public Menu(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive) :
            base (pos, tex, srcRect, isActive)
        {
            this.pos = pos;
            this.tex = tex;
            this.srcRect = srcRect;
            this.isActive = isActive;

            startButton.tex = Textures.startButton;
            optionsButton.tex = Textures.optionsButton;
            exitButton.tex = Textures.exitButton;

            startButton = new MenuButton(pos, tex, srcRect, isActive);
            optionsButton = new MenuButton(pos, tex, srcRect, isActive);
            exitButton = new MenuButton(pos, tex, srcRect, isActive);
        }

        public void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();
            CheckButtons();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            startButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
        }

        public void CheckButtons()
        {
            if (startButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.X) || optionsButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.X)
                || exitButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.X))
            {
                isActive = true;
            }
            else
                isActive = false;

            if(KeyMouseReader.LeftClick() && isActive == true && startButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.X))
            {
                Game1.currentGameState = Game1.gameState.inGame;
            }
            else if (KeyMouseReader.LeftClick() && isActive == true && optionsButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.X))
            {

            }
            else if (KeyMouseReader.LeftClick() && isActive == true && exitButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.X))
            {
                
            }
        }
        
    }
}
