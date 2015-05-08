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
            optionsButton = new MenuButton(new Vector2(200, 100), Textures.optionsButton, new Rectangle(0, 0, 600, 255), true);
            exitButton = new MenuButton(new Vector2(300, 100), Textures.exitButton, new Rectangle(0, 0, 600, 255), true);
        }

        public void Update(GameTime gameTime)
        {
            CheckButtons();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            startButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void CheckButtons()
        {

            if (KeyMouseReader.LeftClick() && startButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                Game1.currentGameState = Game1.gameState.inGame;
            }
            if (KeyMouseReader.LeftClick() && optionsButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                Game1.currentGameState = Game1.gameState.options;
            }
            if (KeyMouseReader.LeftClick() && exitButton.srcRect.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                
            }
        }
        
    }
}
