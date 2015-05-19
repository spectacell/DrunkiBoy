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
        public MenuButton exitButton;
        public MenuButton instructionsButton;
        public MenuButton highscoreButton;
        public MenuButton levelEditorButton;
        private Instructions instructions;
        public static bool showInstructions;

        public Menu(Vector2 pos, Texture2D tex, bool isActive) :
            base (pos, tex, isActive)
        {
            startButton = new MenuButton(new Vector2(100, 100), Textures.startButton, true);
            instructionsButton = new MenuButton(new Vector2(200, 100), Textures.instructionsButton, true);
            levelEditorButton = new MenuButton(new Vector2(300, 100), Textures.levelEditorButton, true);
            highscoreButton = new MenuButton(new Vector2(400, 100), Textures.highscoreButton, true);
            exitButton = new MenuButton(new Vector2(500, 100), Textures.exitButton, true);
            instructions = new Instructions(Vector2.Zero, Textures.menuInstructionsPage, true);
        }

        public void Update(GameTime gameTime)
        {
            if (showInstructions)
            {
                instructions.Update();
            }
            else
            {
                CheckButtons();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (showInstructions)
            {
                instructions.Draw(spriteBatch);
            }
            else 
            { 
                startButton.Draw(spriteBatch);
                instructionsButton.Draw(spriteBatch);
                highscoreButton.Draw(spriteBatch);
                levelEditorButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public void CheckButtons()
        {
            if (KeyMouseReader.LeftClick() && startButton.BoundingBox.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                Game1.currentGameState = Game1.gameState.inGame;
            }
            if (KeyMouseReader.LeftClick() && instructionsButton.BoundingBox.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                showInstructions = true;
            }
            if (KeyMouseReader.LeftClick() && exitButton.BoundingBox.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                Game1.exitgame = true;
            }
            if (KeyMouseReader.LeftClick() && highscoreButton.BoundingBox.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                Highscore.highScoreState = Highscore.state.show;
                Game1.currentGameState = Game1.gameState.highScore;
            }
            if (KeyMouseReader.LeftClick() && levelEditorButton.BoundingBox.Contains(KeyMouseReader.mouseState.X, KeyMouseReader.mouseState.Y))
            {
                Game1.currentGameState = Game1.gameState.levelEditor;
            }
        } 
    }
}
