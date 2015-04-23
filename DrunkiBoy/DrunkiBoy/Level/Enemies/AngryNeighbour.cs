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
        private int speed = 100;
        private bool facingRight;
        public AngryNeighbour(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            this.type = "angryNeighbour";

            health = Constants.health_angryNeightbour;
            movement.X = 1;
            movement.Y = 0;
        }
        public override void Update(GameTime gameTime)
        {
            pos += (float)gameTime.ElapsedGameTime.TotalSeconds * movement * speed;
            if (activePlatform != null && (pos.X >= activePlatform.pos.X + activePlatform.BoundingBox.Width - 57 || pos.X <= activePlatform.pos.X))
            {
                movement.X *= -1;
                facingRight = !facingRight;
            }
            base.Update(gameTime);
            AddGravity();
            CheckIfOnPlatform();
            SwitchFacing();
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
    }
}