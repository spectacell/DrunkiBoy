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
        public static int currentLevel = 0; //0 �r level 1
        private Level level;
        private LevelEditor levelEditor;
        Menu menu;
        //Options options;

        public enum gameState { inGame, levelComplete, gameOver, levelEditor, menu, options, highScore };
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
            menu = new Menu(Vector2.Zero, Textures.menuBackground, true);
            level = new Level(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
            levelEditor = new LevelEditor(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
            gui = new GUI();
            currentGameState = gameState.menu;
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
                gui = new GUI();
                currentGameState = gameState.inGame;
            }
            if (KeyMouseReader.KeyPressed(Keys.F3))
            {
                levelEditor = new LevelEditor(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
                currentGameState = gameState.levelEditor;
            }

            KeyMouseReader.Update();
            switch (currentGameState)
            {
                case gameState.menu:
                    IsMouseVisible = true;
                    menu.Update(gameTime);
                    break;

                //case gameState.options:
                //    IsMouseVisible = true;
                //    options.Update(gameTime);
                //    break;

                case gameState.inGame:
                    IsMouseVisible = false;
                    level.Update(gameTime);
                    gui.Update(gameTime);
                    break;

                case gameState.levelEditor:
                    IsMouseVisible = false;
                    levelEditor.Update(gameTime);
                    break;
                case gameState.levelComplete:
                    currentGameState = gameState.gameOver;
                    break;
                case gameState.gameOver:
                    currentGameState = gameState.highScore;
                    break;
                case gameState.highScore:
                    Highscore.Update();
                    break;
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentGameState)
            {
                case gameState.menu:
                    menu.Draw(spriteBatch);
                    break;

                case gameState.inGame:
                    level.Draw(spriteBatch);
                    gui.Draw(spriteBatch);                    
                    break;

                case gameState.levelEditor:
                    
                    levelEditor.Draw(spriteBatch);
                    break;
                    case gameState.levelComplete:

                    break;
                case gameState.gameOver:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Textures.gameOverScreen, Vector2.Zero, Color.White);
                    spriteBatch.End();
                    break;
                case gameState.highScore:
                    Highscore.Draw(spriteBatch);
                    break;
            }
            
            base.Draw(gameTime);
        }
    }
}
