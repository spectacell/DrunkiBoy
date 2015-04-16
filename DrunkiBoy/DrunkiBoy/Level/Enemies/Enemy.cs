using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DrunkiBoy
{
    abstract class Enemy : AnimatedObject
    {
        protected int health;

        public Enemy(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {

        }
        public void LoseHealth()
        {
            if (health > 0)
            {
                health--;
            }

            if (health <= 0)
            {
                isActive = false;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.Draw(tex, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            }
        }
    }
}
