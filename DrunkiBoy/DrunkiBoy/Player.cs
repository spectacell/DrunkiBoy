using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrunkiBoy
{
    class Player : AnimatedObject
    {
        public Vector2 teleportTo;
        const int playerSpeed = 80;
        KeyboardState ks;
        public int livesLeft, defaultLives = 3;
        public int jumpHeight = 12;
        public Vector2 currentSpawnPos;
        public Vector2 playerMovement;
        public bool onShroom, teleporting;
        private bool playerDead;
        private double spawnTimer, spawnTimerDefault = 750;
        private bool movingLeft;

        public Player(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames)
            : base(pos, tex, srcRect, isActive, nrFrames)
        {

        }
        public override void Update(GameTime gameTime)
        {
            PlayerMovement(gameTime);
            AddFriction(facing);

            //PlayerJumping();
            //CheckIfPlayerIsOnPlatform();
            //AnimateWhenInAir(gameTime);

            base.Update(gameTime);
        }

        private void PlayerMovement(GameTime gameTime)
        {
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Left) && pos.X > -(Game1.windowWidth / 2) && !playerDead && !teleporting)
            {
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                movement.X -= 1;
                facing = 0;
            }
            if (KeyMouseReader.keyState.IsKeyDown(Keys.Right) && !playerDead && !teleporting)
            {
                timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
                movement.X += 1;
                facing = 1;
            }
            pos += movement * (float)gameTime.ElapsedGameTime.TotalSeconds * playerSpeed;
            Console.WriteLine(movement);
            //pos.X = MathHelper.Clamp(pos.X, -(Game1.windowWidth / 2), Level.currentLevel.levelLength - srcRect.Width);
        }
        private void AddFriction(int facing)
        {
            if (facing == 0) //Moving Left
                movement.X += movement.X * 0.2f;
            else
                movement.X -= movement.X * 0.2f;
        }

        //private void PlayerJumping()
        //{
        //    if (KeyMouseReader.KeyPressed(Keys.Up) && activePlatform != null && !teleporting)
        //    {
        //        if (onShroom)
        //        {
        //            movement.Y = -jumpHeight - (0.5f * (activePlatform.pos.Y - activePlatform.orgPos.Y)); 
        //            onShroom = false;
        //            if (activePlatform.pos.Y != activePlatform.orgPos.Y)
        //            {
        //                activePlatform.pos.Y = pos.Y + srcRect.Height;
        //            }
        //        }
        //        else
        //        {
        //            movement.Y += -jumpHeight;
        //            activePlatform = null;
        //        }
        //    }
        //}

        public void SetPlayerDead(bool timeRanOut)
        {
            playerDead = true;
            movement = Vector2.Zero;
            if (livesLeft > 1)
            {
                livesLeft--;
            }
            else
            {
                //Game1.currentGameState = Game1.gameState.GameOver;
            }
        }

        //private void CheckIfPlayerIsOnPlatform()
        //{
        //    if (activePlatform != null)
        //    {
        //        if (!(BottomBoundingBox.Intersects(activePlatform.TopBoundingBox)))
        //        {
        //            activePlatform = null;
        //            onShroom = false;
        //        }
        //    }
        //}

        //private void AnimateWhenInAir(GameTime gameTime)
        //{
        //    if (activePlatform == null & !teleporting)
        //    {
        //        timeTilNextFrame -= gameTime.ElapsedGameTime.TotalMilliseconds;
        //    }
        //}
        public void AddLife()
        {
            if (livesLeft < defaultLives)
            {
                livesLeft++;
            }
        }


    }
}
