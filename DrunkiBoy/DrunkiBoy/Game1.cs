using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DrunkiBoy
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static int windowWidth, windowHeight;
        public static GUI gui;
        public static int currentLevel = 0; //0 är level 1
        private Level level;
        private LevelEditor levelEditor;

        public enum gameState { inGame, levelComplete, gameOver, levelEditor };
        public static gameState currentGameState = gameState.levelEditor;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            windowHeight = Window.ClientBounds.Height;
            windowWidth = Window.ClientBounds.Width;

            Textures.LoadContent(Content);
            Constants.LoadContent(Content);

            level = new Level(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
            levelEditor = new LevelEditor(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
            gui = new GUI();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (KeyMouseReader.KeyPressed(Keys.Escape))
                this.Exit();

            if (KeyMouseReader.KeyPressed(Keys.F2))
            {
                level = new Level(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
                currentGameState = gameState.inGame;
            }
            if (KeyMouseReader.KeyPressed(Keys.F3))
            {
                levelEditor = new LevelEditor(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
                currentGameState = gameState.levelEditor;
            }

            //TESTING METOD
            if (KeyMouseReader.KeyPressed(Keys.A))
            {
                Player.activePowerUp = 1;
                gui.ShowPowerUpCounter(1);
                gui.BlinkHealthBar();
            }
            //---------------

            KeyMouseReader.Update();
            switch (currentGameState)
            {
                case gameState.inGame:
                    level.Update(gameTime);
                    gui.Update(gameTime);
                    break;

                case gameState.levelEditor:
                    
                    levelEditor.Update(gameTime);
                    break;
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentGameState)
            {
                case gameState.inGame:
                    level.Draw(spriteBatch);
                    gui.Draw(spriteBatch);                    
                    break;

                case gameState.levelEditor:
                    
                    levelEditor.Draw(spriteBatch);
                    break;
            }
            
            base.Draw(gameTime);
        }
    }
}
