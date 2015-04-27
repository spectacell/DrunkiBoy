using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class AngryNeighbour : Enemy
    {
        private Texture2D healthBarTex;
        private double healthBarBlinkTimer, healthBarBlinkTimerDefault = 150;
        private int speed = 100;
        private bool facingRight;
        public AngryNeighbour(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            this.type = "angryNeighbour";

            health = Constants.health_angryNeightbour;
            movement.X = 1;
            movement.Y = 0;
            healthBarTex = Textures.angry_neighbour_HB_red;
        }
        public override void Update(GameTime gameTime)
        {
            pos += (float)gameTime.ElapsedGameTime.TotalSeconds * movement * speed;
            if (activePlatform != null && (pos.X >= activePlatform.pos.X + activePlatform.BoundingBox.Width - 57 || pos.X <= activePlatform.pos.X))
            {
                ChangeDirection();
            }
            base.Update(gameTime);
            AddGravity(0.6f);
            CheckIfOnPlatform();
            SwitchFacing();
            HealthBarBlinking(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Health bar
            Vector2 healthBarPos = new Vector2(pos.X, pos.Y - 25);
            spriteBatch.Draw(healthBarTex, healthBarPos, Color.White);
            float greenBarWidth = (health / (float)Constants.health_angryNeightbour) * Textures.angry_neighbour_HB_green.Width;
            spriteBatch.Draw(Textures.angry_neighbour_HB_green, new Vector2(healthBarPos.X + 2, healthBarPos.Y + 2), new Rectangle(0, 0, (int)greenBarWidth, Textures.angry_neighbour_HB_green.Height), Color.White);
            
            base.Draw(spriteBatch);
        }
        public void ChangeDirection()
        {
            movement.X *= -1;
            facingRight = !facingRight;
        }

        private void SwitchFacing()
        {
            if (facingRight)
            {
                facing = 0;
            }
            else
            {
                facing = 1;
            }
        }
        private void CheckIfOnPlatform()
        {
            if (activePlatform != null)
            {
                if (!(BottomBoundingBox.Intersects(activePlatform.TopBoundingBox)))
                {
                    activePlatform = null;
                }
            }
        }
        /// <summary>
        /// Ändrar healthBar-texturen om healthBarBlinkTimer är >= 0 så att det ser ut som att den blinkar till
        /// </summary>
        /// <param name="gameTime"></param>
        private void HealthBarBlinking(GameTime gameTime)
        {
            if (healthBarBlinkTimer >= 0)
            {
                healthBarTex = Textures.angry_neighbour_HB_blink;
            }
            else
            {
                healthBarTex = Textures.angry_neighbour_HB_red;
            }
            healthBarBlinkTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        /// <summary>
        /// I Player klassen så sätts healthBarBlinking till true när player ökar/minskar health. Det aktiverar timern här så att healthBar blinkar till
        /// </summary>
        public void BlinkHealthBar()
        {
            healthBarBlinkTimer = healthBarBlinkTimerDefault;
        }
    }
}