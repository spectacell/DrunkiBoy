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
        Menu menu;
        public static Boolean exitgame = false;

        public enum gameState { inGame, levelComplete, gameOver, levelEditor, menu, options, highScore };
        public static gameState currentGameState;
        private bool isMuted;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 780;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            windowHeight = Window.ClientBounds.Height;
            windowWidth = Window.ClientBounds.Width;
            Sound.LoadContent(Content);
            Textures.LoadContent(Content);
            Constants.LoadContent(Content);
            menu = new Menu(Vector2.Zero, Textures.menuBackground, true);
            level = new Level(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
            levelEditor = new LevelEditor(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
            gui = new GUI();
            currentGameState = gameState.menu;
            MediaPlayer.IsRepeating = true;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (exitgame) this.Exit();

            if (KeyMouseReader.KeyPressed(Keys.Escape))
                currentGameState = gameState.menu;

            if (KeyMouseReader.KeyPressed(Keys.F2))
            {
                currentLevel = 0;
                level = new Level(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
                gui = new GUI();
                currentGameState = gameState.inGame;
                MediaPlayer.Play(Sound.song);
            }
            if (KeyMouseReader.KeyPressed(Keys.F3))
            {
                levelEditor = new LevelEditor(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
                currentGameState = gameState.levelEditor;
            }
            if (KeyMouseReader.KeyPressed(Keys.F4))
            {
                Highscore.highScoreState = Highscore.state.show;
                currentGameState = gameState.highScore;
            }
            if (KeyMouseReader.KeyPressed(Keys.M))
            {
                ToogleMute();
            }
            KeyMouseReader.Update();
            switch (currentGameState)
            {
                case gameState.menu:
                    IsMouseVisible = true;
                    menu.Update(gameTime);
                    break;

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
                    if (currentLevel == 1)
                    {
                        currentGameState = gameState.gameOver;
                        break;
                    }
                    currentLevel++;
                    level.LoadContent(Constants.LEVELS[currentLevel], false);
                    gui = new GUI();
                    currentGameState = gameState.inGame;
                    break;
                case gameState.gameOver:
                    Sound.gameOver.Play();
                    Highscore.highScoreState = Highscore.state.enteringNewHighScore;
                    currentGameState = gameState.highScore;
                    break;
                case gameState.highScore:
                    IsMouseVisible = true;
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
                    level = new Level(GraphicsDevice, Constants.LEVELS[currentLevel], Content);
                    break;
                case gameState.highScore:
                    Highscore.Draw(spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }
        private void ToogleMute()
        {
            if (!isMuted)
            {
                MediaPlayer.IsMuted = true;
                isMuted = true;
            }
            else
            {
                MediaPlayer.IsMuted = false;
                isMuted = false;
            }
        }
    }
}
